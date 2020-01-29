using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetArchTest.Rules;

namespace TestMe.UserManagement.Tests.Architecture
{
    [TestClass]
    public class EncapsulationTests
    {
        static readonly Assembly UserManagementAssembly = typeof(TestUtils).Assembly;


        [TestMethod]
        public void PersistenceIsNotAccessibleFromOutsideOfModuleExceptOfDbContext()
        {
            var result = Types.InAssembly(UserManagementAssembly)
                              .That()
                              .ResideInNamespace("TestMe.UserManagement.Persistence")
                              .And()
                              .DoNotHaveNameStartingWith("IServiceCollectionExtensions")
                              .And()
                              .DoNotHaveNameStartingWith("IServiceProviderExtensions")
                              .And()
                              .DoNotHaveNameEndingWith("DbContext")
                              .Should()
                              .NotBePublic()
                              .GetResult();
            Assert.IsTrue(result.IsSuccessful);
        }

       


        [TestMethod]
        public void InfrastructureIsNotAccessibleFromOutsideOfModule()
        {
            var result = Types.InAssembly(UserManagementAssembly)
                              .That()
                              .ResideInNamespace("TestMe.UserManagement.Infrastructure")
                              .And()
                              .DoNotHaveNameStartingWith("IServiceCollectionExtensions")
                              .Should()
                              .NotBePublic()
                              .GetResult();
            Assert.IsTrue(result.IsSuccessful);
        }
    }
}