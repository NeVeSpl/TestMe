using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestMe.TestCreation.App.Tests.Input
{
    public class CreateQuestionItem
    {
        public long UserId { get; set; }
        public long TestId { get; set; }
        public long QuestionId { get; set; }
    }
}
