using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.App.RequestHandlers.QuestionsCatalogs.DeleteCatalog
{
    internal class DeleteCatalogHandler : IRequestHandler<DeleteCatalogCommand, Result>
    {
        private readonly ITestCreationUoW uow;


        public DeleteCatalogHandler(ITestCreationUoW uow)
        {
            this.uow = uow;
        }

        public async Task<Result> Handle(DeleteCatalogCommand command, CancellationToken cancellationToken)
        {
            var owner = uow.Owners.GetById(command.UserId);
            var catalog = uow.QuestionsCatalogs.GetById(command.CatalogId, includeQuestions: true);

            if (catalog == null)
            {
                return Result.NotFound();
            }
            if (catalog.OwnerId != command.UserId)
            {
                return Result.Unauthorized();
            }

            owner.DeleteQuestionsCatalog(catalog);
            await uow.Save();

            return Result.Ok();
        }
    }
}