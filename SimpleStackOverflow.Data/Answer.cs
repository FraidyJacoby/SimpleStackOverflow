using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStackOverflow.Data
{
    public class Answer
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
