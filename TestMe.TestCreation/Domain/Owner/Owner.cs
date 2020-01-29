using System;
using System.Collections.Generic;
using System.Text;
using TestMe.BuildingBlocks.Domain;
using TestMe.SharedKernel.Domain;

namespace TestMe.TestCreation.Domain
{
    internal sealed class Owner
    {
        private readonly List<QuestionsCatalog> _questionsCatalogs = new List<QuestionsCatalog>();
        private readonly List<TestsCatalog> _testsCatalogs = new List<TestsCatalog>();

        
        public long OwnerId { get; private set; }
        public int QuestionsCatalogsCount { get; private set; } = 0;
        public IReadOnlyList<QuestionsCatalog> QuestionsCatalogs
        {
            get => _questionsCatalogs;
        }
        public IReadOnlyList<TestsCatalog> TestsCatalogs
        {
            get => _testsCatalogs;
        }
        public MembershipLevel MembershipLevel { get; set; }


        private Owner()
        {

        }


        public QuestionsCatalog AddQuestionsCatalog(string catalogName, IAddQuestionsCatalogPolicy addQuestionsCatalogPolicy)
        {
            if (addQuestionsCatalogPolicy.CanAddQuestionsCatalog(QuestionsCatalogsCount))
            {
                var catalog = QuestionsCatalog.Create(catalogName, OwnerId);
                _questionsCatalogs.Add(catalog);
                QuestionsCatalogsCount++;
                return catalog;
            }
            else
            {
                throw new DomainException(DomainExceptions.Limit_of_questions_catalogs_has_been_reached_thus_you_cannot_add_a_new_questions_catalog);
            }
        }

        public TestsCatalog AddTestsCatalog(string catalogName)
        {
            var catalog = TestsCatalog.Create(catalogName, OwnerId);
            _testsCatalogs.Add(catalog);            
            return catalog;
        }



        public static Owner Create(long ownerId, MembershipLevel membershipLevel)
        {
            return new Owner() { OwnerId = ownerId, MembershipLevel = membershipLevel };
        }

        public void DeleteQuestionsCatalog(QuestionsCatalog catalog)
        {           
            catalog.Delete();
            QuestionsCatalogsCount--;            
        }
    }
}
