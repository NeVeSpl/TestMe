using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetArchTest.Rules;

namespace TestMe.TestCreation.Tests.Architecture
{
    [TestClass]
    public class EncapsulationTests
    {
        static readonly Assembly TestCreationAssembly = typeof(TestUtils).Assembly;


        [TestMethod]
        public void DomainIsNotAccessibleFromOutsideOfModule()
        {
            var result = Types.InAssembly(TestCreationAssembly)
                              .That()
                              .ResideInNamespace("TestMe.TestCreation.Domain")
                              .And()
                              .DoNotHaveNameEndingWith("Const")
                              .Should()
                              .NotBePublic()
                              .GetResult();
            Assert.IsTrue(result.IsSuccessful);
        }


        [TestMethod]
        public void PersistenceIsNotAccessibleFromOutsideOfModule()
        {
            var result = Types.InAssembly(TestCreationAssembly)
                              .That()
                              .ResideInNamespace("TestMe.TestCreation.Persistence")
                              .And()
                              .DoNotHaveNameStartingWith("IServiceCollectionExtensions")
                              .And()
                              .DoNotHaveNameStartingWith("IServiceProviderExtensions")
                              .Should()
                              .NotBePublic()
                              .GetResult();
            Assert.IsTrue(result.IsSuccessful);
        }

        [TestMethod]
        [DataRow("Service")]
        [DataRow("Readers")]
        [DataRow("Handler")]
        public void AppServicesAndReadersAndHandlersAreNotAccessibleFromOutsideOfModule(string end)
        {
            var result = Types.InAssembly(TestCreationAssembly)
                              .That()
                              .ResideInNamespace("TestMe.TestCreation.App")
                              .And()
                              .AreClasses()
                              .And()
                              .HaveNameEndingWith(end)
                              .Should()
                              .NotBePublic()
                              .GetResult();
            Assert.IsTrue(result.IsSuccessful);
        }


        [TestMethod]
        public void InfrastructureIsNotAccessibleFromOutsideOfModule()
        {
            var result = Types.InAssembly(TestCreationAssembly)
                              .That()
                              .ResideInNamespace("TestMe.TestCreation.Infrastructure")
                              .And()
                              .DoNotHaveNameStartingWith("IServiceCollectionExtensions")
                              .Should()
                              .NotBePublic()
                              .GetResult();
            Assert.IsTrue(result.IsSuccessful);
        }
    }
}