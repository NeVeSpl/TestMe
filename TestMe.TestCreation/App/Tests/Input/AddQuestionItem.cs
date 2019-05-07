using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestMe.TestCreation.App.Tests.Input
{
    public class AddQuestionItem
    {
        [Required]
        public long? QuestionId { get; set; }
    }
}
