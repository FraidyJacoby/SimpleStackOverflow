using SimpleStackOverflow.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStackOverflow.Web.Models
{
    public class ViewQuestionViewModel
    {
        public Question Question { get; set; }
        public bool UserIsLoggedIn { get; set; }
        public User User { get; set; }
    }
}
