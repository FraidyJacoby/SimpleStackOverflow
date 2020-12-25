using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SimpleStackOverflow.Data;
using SimpleStackOverflow.Web.Models;

namespace SimpleStackOverflow.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private string _connectionString;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        public IActionResult Index()
        {
            var repo = new QuestionsRepository(_connectionString);
            var vm = new IndexViewModel
            {
                Questions = repo.GetQuestions(),
                UserIsLoggedIn = User.Identity.IsAuthenticated
            };
            return View(vm);
        }

        [Authorize]
        public IActionResult AskAQuestion()
        {
            var repo = new UsersRepository(_connectionString);
            var vm = new AskAQuestionViewModel 
            {
                User = repo.GetByEmail(User.Identity.Name)
            };

            return View(vm);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddQuestion(Question question, string tagString)
        {
            var repo = new QuestionsRepository(_connectionString);
            repo.AddQuestion(question, tagString);
            return Redirect("/");
        }

        public IActionResult ViewQuestion(int questionId)
        {
            var questionsRepo = new QuestionsRepository(_connectionString);
            var userRepo = new UsersRepository(_connectionString);
            var vm = new ViewQuestionViewModel();
            var question = questionsRepo.GetQuestionById(questionId);
            if (question == null)
            {
                return Redirect("/");
            }
            vm.Question = question;
            vm.UserIsLoggedIn = User.Identity.IsAuthenticated;
            if (vm.UserIsLoggedIn)
            {
                vm.User = userRepo.GetByEmail(User.Identity.Name);
            }
            return View(vm);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddAnswer(Answer answer)
        {
            var repo = new QuestionsRepository(_connectionString);
            repo.AddAnswer(answer);
            return Redirect($"/home/viewQuestion?questionId={answer.QuestionId}");
        }

        [Authorize]
        [HttpPost]
        public void Like(Like like)
        {
            var repo = new QuestionsRepository(_connectionString);
            repo.AddLike(like);
        }

        public int GetLikes(int questionId)
        {
            var repo = new QuestionsRepository(_connectionString);
            return repo.GetLikesCount(questionId);
        }

        public bool CheckIfLiked(int questionId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return false;
            }

            var usersRepo = new UsersRepository(_connectionString);
            var user = usersRepo.GetByEmail(User.Identity.Name);

            var questionsRepo = new QuestionsRepository(_connectionString);
            return questionsRepo.DidLike(questionId, user.Id);
        }
    }
}
