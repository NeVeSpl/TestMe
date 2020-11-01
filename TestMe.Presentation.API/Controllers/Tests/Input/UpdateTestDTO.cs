using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TestMe.TestCreation.App.RequestHandlers.Tests.UpdateTest;
using TestMe.TestCreation.Domain;

namespace TestMe.Presentation.API.Controllers.Tests.Input
{
    public class UpdateTestDTO
    {
        [StringLength(maximumLength: TestConst.TitleMaxLength)]
        public string Title { get; set; } = string.Empty;



        public UpdateTestCommand CreateCommand(long testId)
        {
            return new UpdateTestCommand()
            {               
                Title = Title,              
                TestId = testId
            };
        }
    }
}
