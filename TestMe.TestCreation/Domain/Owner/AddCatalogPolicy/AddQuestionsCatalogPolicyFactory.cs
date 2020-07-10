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
                MembershipLevel.Regular => 15,
                MembershipLevel.Silver => 18,
                MembershipLevel.Gold => 23,
                _ => throw new NotImplementedException(),
            };
            return new AddQuestionsCatalogPolicy(maxNumberOfQuestionsCatalogs);
        }
    }
}
