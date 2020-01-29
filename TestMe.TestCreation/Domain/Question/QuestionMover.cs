using System;
using System.Collections.Generic;
using System.Text;
using TestMe.BuildingBlocks.Domain;

namespace TestMe.TestCreation.Domain
{
    internal static class QuestionMover
    {
        public static void MoveQuestionToCatalog(Question question, long targetCatalogId, IQuestionsCatalogRepository repository, IAddQuestionPolicy policy)
        {
            QuestionsCatalog source = repository.GetById(question.CatalogId, includeQuestions: true);
            QuestionsCatalog destination = repository.GetById(targetCatalogId, includeQuestions: true);

            if (destination == null)
            {
                throw new DomainException(DomainExceptions.Catalog_not_found);                
            }

            if (source.OwnerId != destination.OwnerId)
            {
                throw new DomainException(DomainExceptions.Question_can_not_be_moved_to_catalog_that_you_do_not_own);
            }

            source.RemoveQuestion(question);
            destination.AddQuestion(question, policy);
        }
    }
}
