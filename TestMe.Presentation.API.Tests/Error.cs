using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestMe.Presentation.API.Tests.Utils;


namespace TestMe.Presentation.API.Tests
{
    [TestClass]
    public class Error : BaseFixture
    {
        private const string EndpointName = "Error";
        private HttpClient client;


        [TestInitialize]
        public void TestInitialize()
        {
            factory = new ApiFactory(ApiFactory.DatabaseType.SQLiteInMemory);
            client = factory.CreateClient(ValidToken);
        }


        // todo : how to test error controler?       
        // DbUpdateConcurrencyException
        // DomainException
        // Unexpected exception
    }
}
