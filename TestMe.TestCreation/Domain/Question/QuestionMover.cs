using System;
using System.Collections.Generic;
using System.Text;
using TestMe.SharedKernel.Domain;

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
                throw new DomainException("Catalog not found");                
            }

            if (source.OwnerId != destination.OwnerId)
            {
                throw new DomainException("Question can not be moved to catalog that you do not own");
            }

            source.RemoveQuestion(question);
            destination.AddQuestion(question, policy);
        }
    }
}
