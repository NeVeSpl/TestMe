using Microsoft.Extensions.DependencyInjection;
using TestMe.TestCreation.App.EventHandlers;
using TestMe.TestCreation.App.Questions;
using TestMe.TestCreation.App.QuestionsCatalogs;
using TestMe.TestCreation.App.Tests;
using TestMe.TestCreation.App.TestsCatalogs;

namespace TestMe.TestCreation.App
{
    public static class IServiceCollectionExtensionsFromTestCreationApp
    {
        public static void AddTestCreationApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IQuestionsCatalogsService, QuestionsCatalogsService>();
            services.AddTransient<QuestionsCatalogReader>();

            services.AddTransient<ITestsCatalogsService, TestsCatalogsService>();
            services.AddTransient<TestsCatalogReader>();

            services.AddTransient<IQuestionsService, QuestionsService>();
            services.AddTransient<QuestionReader>();

            services.AddTransient<ITestsService, TestsService>();
            services.AddTransient<TestReader>();
          
            services.AddTransient<UserCreatedEventHandler>();         
        }
    }
}