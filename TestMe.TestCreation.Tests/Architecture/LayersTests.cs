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
                              .HaveDependencyOnAny( 
                                "TestMe.TestCreation.Persistence",
                                "TestMe.TestCreation.App",
                                "TestMe.TestCreation.Infrastructure",
                                "TestMe.BuildingBlocks.App",
                                "TestMe.BuildingBlocks.EventBus",
                                "TestMe.BuildingBlocks.Persistence",
                                "TestMe.UserManagement.IntegrationEvents",
                                "Microsoft.EntityFrameworkCore"
                              ) 
                              .GetResult();
            Assert.IsTrue(result.IsSuccessful, "Domain has lost its independence!");                             
        }

        [TestMethod]
        public void PersistenceDoesNotHaveAnyDependecyOn()
        {
            var result = Types.InAssembly(TestCreationAssembly)
                             .That()
                             .ResideInNamespace("TestMe.TestCreation.Persistence")                             
                             .ShouldNot()
                             .HaveDependencyOnAny(
                                "TestMe.TestCreation.Infrastructure",
                                "TestMe.TestCreation.App",
                                "TestMe.BuildingBlocks.App",                                
                                "TestMe.BuildingBlocks.Domain",
                                "TestMe.UserManagement.IntegrationEvents"                             
                             )
                             .GetResult();
            Assert.IsTrue(result.IsSuccessful, "Persistence has lost its independence!");
        }

        [TestMethod]
        public void AppDoesNotHaveAnyDependecyOn()
        {
            var result = Types.InAssembly(TestCreationAssembly)
                             .That()
                             .ResideInNamespace("TestMe.TestCreation.App")
                             .ShouldNot()
                             .HaveDependencyOnAny(                               
                                "TestMe.BuildingBlocks.Domain",                               
                                "TestMe.BuildingBlocks.Persistence"                            
                             )
                             .GetResult();
            Assert.IsTrue(result.IsSuccessful, "App has lost its independence!");
        }
    }
}
