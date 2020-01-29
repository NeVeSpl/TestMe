using System;
using System.Collections.Generic;
using System.Text;
using TestMe.TestCreation.App.Tests.Input;

namespace TestMe.Presentation.API.Controllers.Tests.Input
{
    public class UpdateTestDTO : CreateTestDTO
    {
      



        public UpdateTest CreateCommand(long userId, long testId)
        {
            return new UpdateTest()
            {
                CatalogId = CatalogId!.Value,
                Title = Title,
                UserId = userId,
                TestId = testId
            };
        }
    }
}
