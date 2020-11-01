using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestMe.TestCreation.App.RequestHandlers.Questions.UpdateQuestion;
using TestMe.TestCreation.Domain;

namespace TestMe.Presentation.API.Controllers.Questions.Input
{
    public class UpdateQuestionDTO 
    {
        [StringLength(maximumLength: QuestionConst.ContentMaxLength)]
        public string Content { get; set; } = string.Empty;

        public List<UpdateAnswerDTO> Answers { get; set; } = new List<UpdateAnswerDTO>();

        [Required]
        public long? CatalogId { get; set; }

        /// <summary>
        /// When ConcurrencyToken is not provided update will be forced (it will succeed even if concurrent edit happened)
        /// </summary>
        public uint? ConcurrencyToken { get; set; }



        public UpdateQuestionWithAnswersCommand CreateCommand(long questionId)
        {
            return new UpdateQuestionWithAnswersCommand()
            {
                Content = Content,
                Answers = Answers.ConvertAll(thisAnswer => thisAnswer.CreateAnswer()),
                CatalogId = CatalogId!.Value,
              
                QuestionId = questionId,
                ConcurrencyToken = ConcurrencyToken
            };
        }
    }
}
