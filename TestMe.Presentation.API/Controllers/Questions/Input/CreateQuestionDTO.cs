using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestMe.TestCreation.App.Questions.Input;
using TestMe.TestCreation.Domain;

namespace TestMe.Presentation.API.Controllers.Questions.Input
{
    public class CreateQuestionDTO : QuestionBaseDTO<CreateAnswerDTO>
    {
        public CreateQuestion CreateCommand(long userId)
        {
            return new CreateQuestion()
            {
                Content = Content,
                Answers = Answers.ConvertAll(thisAnswer => thisAnswer.CreateAnswer()),
                CatalogId = CatalogId!.Value,
                UserId = userId,
               
            };
        }
    }
}
