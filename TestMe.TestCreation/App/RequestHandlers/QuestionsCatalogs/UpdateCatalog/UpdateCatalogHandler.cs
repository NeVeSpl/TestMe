using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.App.RequestHandlers.QuestionsCatalogs.UpdateCatalog
{
    internal class UpdateCatalogHandler : IRequestHandler<UpdateCatalogCommand, Result>
    {
        private readonly ITestCreationUoW uow;


        public UpdateCatalogHandler(ITestCreationUoW uow)
        {
            this.uow = uow;
        }



        public async Task<Result> Handle(UpdateCatalogCommand command, CancellationToken cancellationToken)
        {
            QuestionsCatalog catalog = uow.QuestionsCatalogs.GetById(command.CatalogId);

            if (catalog == null)
            {
                return Result.NotFound();
            }
            if (catalog.OwnerId != command.UserId)
            {
                return Result.Unauthorized();
            }

            catalog.Name = command.Name;
            await uow.Save();

            return Result.Ok();
        }
    }
}