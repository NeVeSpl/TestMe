using Microsoft.Extensions.DependencyInjection;
using TestMe.TestCreation.App.Catalogs;
using TestMe.TestCreation.App.Questions;
using TestMe.TestCreation.App.Tests;

namespace TestMe.TestCreation.App
{
    public static class IServiceCollectionExtensions
    {
        public static void AddTestCreationApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IQuestionsCatalogsService, QuestionsCatalogsService>();
            services.AddTransient<QuestionsCatalogReader, QuestionsCatalogReader>();

            services.AddTransient<ITestsCatalogsService, TestsCatalogsService>();
            services.AddTransient<TestsCatalogReader, TestsCatalogReader>();

            services.AddTransient<IQuestionsService, QuestionsService>();
            services.AddTransient<QuestionReader, QuestionReader>();

            services.AddTransient<ITestsService, TestsService>();
            services.AddTransient<TestReader, TestReader>();
        }
    }
}