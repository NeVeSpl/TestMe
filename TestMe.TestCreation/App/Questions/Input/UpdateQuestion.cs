using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.App.Questions.Input
{
    public class UpdateQuestion : QuestionBase<UpdateAnswer>
    {
        /// <summary>
        /// When ConcurrencyToken is not provided update will be forced (it will succeed even if concurrent edit happened)
        /// </summary>
        public uint? ConcurrencyToken { get; set; }
    }
}
