using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetArchTest.Rules;

namespace TestMe.UserManagement.Tests.Architecture
{
    [TestClass]
    public class LayersTests
    {
        static readonly Assembly UserManagementAssembly = typeof(TestUtils).Assembly;

        [TestMethod]
        public void DomainIsIndependent()
        {
            var result = Types.InAssembly(UserManagementAssembly)
                              .That()
                              .ResideInNamespace("TestMe.UserManagement.Domain")
                              .ShouldNot()
                              .HaveDependenciesOtherThan(
                                "System",
                                "TestMe.UserManagement.Domain",
                                "TestMe.SharedKernel.Domain",
                                "TestMe.BuildingBlocks.Domain",
                                "TestMe.UserManagement.IntegrationEvents"
                              ) 
                              .GetResult();
            Assert.IsTrue(result.IsSuccessful, "Domain has lost its independence!");                             
        }

        [TestMethod]
        public void PersistenceDoesNotHaveDependenciesOtherThan()
        {
            var result = Types.InAssembly(UserManagementAssembly)
                             .That()
                             .ResideInNamespace("TestMe.UserManagement.Persistence")                             
                             .ShouldNot()
                             .HaveDependenciesOtherThan(
                                "System",
                                "TestMe.UserManagement.Persistence",
                                "TestMe.UserManagement.Domain",
                                "TestMe.BuildingBlocks.Persistence",
                                "TestMe.BuildingBlocks.EventBus",
                                "Microsoft.EntityFrameworkCore",
                                "Microsoft.Extensions.DependencyInjection"
                             )
                             .GetResult();
            Assert.IsTrue(result.IsSuccessful, "Persistence has lost its independence!");
        }

        [TestMethod]
        public void AppDoesNotHaveDependenciesOtherThan()
        {
            var result = Types.InAssembly(UserManagementAssembly)
                             .That()
                             .ResideInNamespace("TestMe.UserManagement.App")
                             .ShouldNot()
                             .HaveDependenciesOtherThan(
                                "System",
                                "TestMe.UserManagement.App",
                                "TestMe.UserManagement.Persistence",
                                "TestMe.UserManagement.Domain",                            
                                "TestMe.BuildingBlocks.App",
                                "TestMe.BuildingBlocks.Domain",
                                "TestMe.SharedKernel.Domain",
                                "Microsoft.EntityFrameworkCore",                            
                                "Microsoft.Extensions.DependencyInjection",
                                "AutoMapper"
                             )
                             .GetResult();
            Assert.IsTrue(result.IsSuccessful, "App has lost its independence!");
        }
    }
}