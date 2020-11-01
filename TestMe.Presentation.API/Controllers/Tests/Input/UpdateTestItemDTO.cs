using System;
using System.Collections.Generic;
using System.Text;

using TestMe.TestCreation.App.RequestHandlers.Tests.UpdateTestItem;

namespace TestMe.Presentation.API.Controllers.Tests.Input
{
    public class UpdateTestItemDTO : CreateTestItemDTO
    {        

        internal UpdateTestItemCommand CreateCommand(long testId, long TestItemId)
        {
            return new UpdateTestItemCommand()
            {
                TestId = testId,
                TestItemId = TestItemId,                
            };
        }
    }
}
