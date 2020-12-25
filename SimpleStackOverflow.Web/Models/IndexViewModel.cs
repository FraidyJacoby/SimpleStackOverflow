using SimpleStackOverflow.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStackOverflow.Web.Models
{
    public class IndexViewModel
    {
        public List<Question> Questions { get; set; }
        public bool UserIsLoggedIn { get; set; }
    }
}
