﻿using TestMe.TestCreation.App.Questions.Input;

namespace TestMe.Presentation.API.Controllers.Questions.Input
{
    public class CreateQuestionDTO : QuestionBaseDTO<CreateAnswerDTO>
    {
        public CreateQuestion CreateCommand()
        {
            return new CreateQuestion()
            {
                Content = Content,
                Answers = Answers.ConvertAll(thisAnswer => thisAnswer.CreateAnswer()),
                CatalogId = CatalogId!.Value, 
            };
        }
    }
}
