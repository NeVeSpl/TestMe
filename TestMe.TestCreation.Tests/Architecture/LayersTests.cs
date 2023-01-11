using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetArchTest.Rules;

namespace TestMe.TestCreation.Tests.Architecture
{
    [TestClass]
    public class LayersTests
    {
        static readonly Assembly TestCreationAssembly = typeof(TestUtils).Assembly;

        [TestMethod]
        public void DomainIsIndependent()
        {
            var result = Types.InAssembly(TestCreationAssembly)
                              .That()
                              .ResideInNamespace("TestMe.TestCreation.Domain")
                              .ShouldNot()
                              .HaveDependencyOtherThan( 
                                "System",
                                "TestMe.TestCreation.Domain",
                                "TestMe.SharedKernel.Domain",
                                "TestMe.BuildingBlocks.Domain"
                              ) 
                              .GetResult(); 
           
            Assert.IsTrue(result.IsSuccessful, "Domain has lost its independence!");                             
        }

        [TestMethod]
        public void PersistenceDoesNotHaveDependenciesOtherThan()
        {
            var result = Types.InAssembly(TestCreationAssembly)
                             .That()
                             .ResideInNamespace("TestMe.TestCreation.Persistence")                             
                             .ShouldNot()
                             .HaveDependencyOtherThan(
                                "System",
                                "TestMe.TestCreation.Persistence",
                                "TestMe.TestCreation.Domain",
                                "TestMe.BuildingBlocks.Persistence",
                                "TestMe.BuildingBlocks.EventBus",
                                "Microsoft.EntityFrameworkCore",
                                "Microsoft.Extensions.DependencyInjection"
                             )
                             .GetResult();
            Assert.IsTrue(result.IsSuccessful, "Persistence has lost its independence!");
        }

        [TestMethod]
        public void AppDoesNotNotHaveDependenciesOtherThan()
        {
            var result = Types.InAssembly(TestCreationAssembly)
                             .That()
                             .ResideInNamespace("TestMe.TestCreation.App")
                             .ShouldNot()
                             .HaveDependencyOtherThan(
                                "System",
                                "TestMe.TestCreation.App",
                                "TestMe.TestCreation.Persistence",
                                "TestMe.TestCreation.Domain",
                                "TestMe.TestCreation.Infrastructure",
                                "TestMe.BuildingBlocks.App",
                                "TestMe.BuildingBlocks.EventBus",
                                "TestMe.UserManagement.IntegrationEvents",
                                "Microsoft.Extensions.DependencyInjection",
                                "Microsoft.EntityFrameworkCore",
                                "MediatR"
                             )
                             .GetResult();
            Assert.IsTrue(result.IsSuccessful, "App has lost its (weak) independence!");
        }
    }
}
