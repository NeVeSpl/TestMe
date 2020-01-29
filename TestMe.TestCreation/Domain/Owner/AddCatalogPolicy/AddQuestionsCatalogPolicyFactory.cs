using System;
using System.Collections.Generic;
using System.Text;
using TestMe.SharedKernel.Domain;

namespace TestMe.TestCreation.Domain
{
    internal static class AddQuestionsCatalogPolicyFactory
    {
        public static IAddQuestionsCatalogPolicy Create(MembershipLevel membershipLevel)
        { 
            int maxNumberOfQuestionsCatalogs = membershipLevel switch
            {
                MembershipLevel.Regular => 5,
                MembershipLevel.Silver => 8,
                MembershipLevel.Gold => 13,
                _ => throw new NotImplementedException(),
            };
            return new AddQuestionsCatalogPolicy(maxNumberOfQuestionsCatalogs);
        }
    }
}
