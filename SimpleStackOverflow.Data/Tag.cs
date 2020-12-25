using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStackOverflow.Data
{
    public class Tag
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public List<QuestionTag> QuestionTags { get; set; }
    }
}
