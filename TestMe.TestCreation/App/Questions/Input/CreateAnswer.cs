using System.ComponentModel.DataAnnotations;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.App.Questions.Input
{
    public class CreateAnswer
    {
        public string Content { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }
    }
}
