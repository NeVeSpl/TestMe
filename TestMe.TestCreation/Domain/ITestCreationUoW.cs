namespace TestMe.TestCreation.Domain
{
    internal interface ITestCreationUoW
    {
        IQuestionRepository Questions { get; }
        IQuestionsCatalogRepository QuestionsCatalogs { get; }
        ITestsCatalogRepository TestsCatalogs { get; }
        ITestRepository Tests { get; }
        IOwnerRepository Owners { get; }

        void Save();
    }
}
