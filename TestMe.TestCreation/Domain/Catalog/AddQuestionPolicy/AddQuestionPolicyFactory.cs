using System;
using System.Collections.Generic;
using System.Text;

namespace TestMe.TestCreation.Domain
{
    internal static class AddQuestionPolicyFactory
    {
        public static IAddQuestionPolicy Create(MembershipLevel membershipLevel)
        {
            int maxNumberOfQuestionsInCatalog = 0;

            switch (membershipLevel)
            {
                case MembershipLevel.Regular:
                    maxNumberOfQuestionsInCatalog = 5;
                    break;
                case MembershipLevel.Silver:
                    maxNumberOfQuestionsInCatalog = 8;
                    break;
                case MembershipLevel.Gold:
                    maxNumberOfQuestionsInCatalog = 13;
                    break;
                default:
                    throw new NotImplementedException();
            }

            return new AddQuestionPolicy(maxNumberOfQuestionsInCatalog);
        }
    }
}
