using System.ComponentModel.DataAnnotations;

namespace TestMe.TestCreation.App.Questions.Input
{
    public class UpdateAnswer : CreateAnswer
    {
        [Required]
        public long? AnswerId
        {
            get;
            set;
        }

    }
}
