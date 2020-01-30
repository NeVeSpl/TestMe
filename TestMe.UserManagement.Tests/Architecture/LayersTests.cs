﻿using System.Reflection;
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
                              .ResideInNamespace("TestMe.TestCreation.Domain")
                              .ShouldNot()
                              .HaveDependencyOnAny(
                                "TestMe.UserManagement.Persistence",
                                "TestMe.UserManagement.App",
                                "TestMe.UserManagement.Infrastructure",
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
            var result = Types.InAssembly(UserManagementAssembly)
                             .That()
                             .ResideInNamespace("TestMe.TestCreation.Persistence")                             
                             .ShouldNot()
                             .HaveDependencyOnAny(
                                "TestMe.UserManagement.Infrastructure",
                                "TestMe.UserManagement.App",
                                "TestMe.BuildingBlocks.App",
                                "TestMe.UserManagement.IntegrationEvents"                             
                             )
                             .GetResult();
            Assert.IsTrue(result.IsSuccessful, "Persistence has lost its independence!");
        }

        [TestMethod]
        public void AppDoesNotHaveAnyDependecyOn()
        {
            var result = Types.InAssembly(UserManagementAssembly)
                             .That()
                             .ResideInNamespace("TestMe.TestCreation.App")
                             .ShouldNot()
                             .HaveDependencyOnAny(
                                "TestMe.UserManagement.Infrastructure",
                                "TestMe.BuildingBlocks.Domain",                               
                                "TestMe.BuildingBlocks.Persistence"                            
                             )
                             .GetResult();
            Assert.IsTrue(result.IsSuccessful, "App has lost its independence!");
        }
    }
}