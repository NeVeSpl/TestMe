using System;
using System.Collections.Generic;
using System.Text;

namespace TestMe.TestCreation.App.Tests.Input
{
    public class DeleteTest
    {
        public long UserId { get; set; }
        public long TestId { get; set; }


        public DeleteTest()
        {

        }

        public DeleteTest(long userId, long testId)
        {
            UserId = userId;
            TestId = testId;
        }
    }
}