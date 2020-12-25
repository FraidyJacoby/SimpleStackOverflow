using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleStackOverflow.Data
{
    public class QuestionsRepository
    {
        private string _connectionString;

        public QuestionsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Question> GetQuestions()
        {
            using(var ctx = new QuestionsContext(_connectionString))
            {
                return ctx.Questions.OrderByDescending(Question => Question.Date).ToList();
            }
        }

        public Question GetQuestionById(int id)
        {
            using (var ctx = new QuestionsContext(_connectionString))
            {
                return ctx.Questions.Include(q => q.User).Include(q => q.Likes)
                    .Include(q => q.Answers).Include(q => q.QuestionTags).ThenInclude(qt => qt.Tag).FirstOrDefault(q => q.Id == id);
            }
        }

        public void AddQuestion(Question question, string tagString)
        {
            using(var ctx = new QuestionsContext(_connectionString))
            {
                ctx.Questions.Add(question);
                ctx.SaveChanges();
                var tags = tagString.Split(" ");
                foreach (string t in tags)
                {
                    var tag = GetTag(t);
                    if (tag == null)
                    {
                        tag = new Tag { Title = t };
                        AddTag(tag);
                    }

                    ctx.QuestionsTags.Add(new QuestionTag
                    {
                        QuestionId = question.Id,
                        TagId = tag.Id
                    });
                }
                ctx.SaveChanges();
            }
        }

        public void AddAnswer(Answer answer)
        {
            using(var ctx = new QuestionsContext(_connectionString))
            {
                ctx.Answers.Add(answer);
                ctx.SaveChanges();
            }
        }

        public void AddLike(Like like)
        {
            using(var ctx = new QuestionsContext(_connectionString))
            {
                ctx.Likes.Add(new Like 
                { 
                    QuestionId = like.QuestionId,
                    UserId = like.UserId
                });
                ctx.SaveChanges();
            }
        }

        public Tag GetTag(string value)
        {
            using (var ctx = new QuestionsContext(_connectionString))
            {
                return ctx.Tags.FirstOrDefault(t => t.Title == value);
            }
        }

        public void AddTag(Tag tag)
        {
            using (var ctx = new QuestionsContext(_connectionString))
            {
                ctx.Tags.Add(tag);
                ctx.SaveChanges();
            }
        }

        public int GetLikesCount(int questionId)
        {
            using (var ctx = new QuestionsContext(_connectionString))
            {
                var result = ctx.Questions.Include(q => q.Likes).FirstOrDefault(q => q.Id == questionId).Likes;
                return result.Count();
            }
        }

        public bool DidLike(int questionId, int userId)
        {
            using (var ctx = new QuestionsContext(_connectionString))
            {
                return ctx.Likes.FirstOrDefault(q => q.QuestionId == questionId && q.UserId == userId) != null;
            }
        }
    }
}
