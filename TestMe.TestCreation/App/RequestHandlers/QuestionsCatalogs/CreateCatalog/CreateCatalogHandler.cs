using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.App.RequestHandlers.QuestionsCatalogs.CreateCatalog
{
    internal class CreateCatalogHandler : IRequestHandler<CreateCatalogCommand, Result<long>>
    {
        private readonly ITestCreationUoW uow;


        public CreateCatalogHandler(ITestCreationUoW uow)
        {
            this.uow = uow;
        }


        public async Task<Result<long>> Handle(CreateCatalogCommand command, CancellationToken cancellationToken)
        {
            Owner owner = uow.Owners.GetById(command.OwnerId);

            if (owner == null)
            {
                return Result.NotFound();
            }

            if (owner.OwnerId != command.UserId)
            {
                return Result.Unauthorized();
            }

            var policy = AddQuestionsCatalogPolicyFactory.Create(owner.MembershipLevel);
            QuestionsCatalog catalog = owner.AddQuestionsCatalog(command.Name, policy);
            await uow.Save();

            return Result.Ok(catalog.CatalogId);
        }
    }
}