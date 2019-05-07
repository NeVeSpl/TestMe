using System;
using System.Collections.Generic;
using System.Text;

namespace TestMe.TestCreation.Domain
{
    internal static class AddQuestionsCatalogPolicyFactory
    {
        public static IAddQuestionsCatalogPolicy Create(MembershipLevel membershipLevel)
        {
            int maxNumberOfQuestionsCatalogs = 0;

            switch (membershipLevel)
            {
                case MembershipLevel.Regular:
                    maxNumberOfQuestionsCatalogs = 5;
                    break;
                case MembershipLevel.Silver:
                    maxNumberOfQuestionsCatalogs = 8;
                    break;
                case MembershipLevel.Gold:
                    maxNumberOfQuestionsCatalogs = 13;
                    break;
                default:
                    throw new NotImplementedException();
            }

            return new AddQuestionsCatalogPolicy(maxNumberOfQuestionsCatalogs);
        }
    }
}
