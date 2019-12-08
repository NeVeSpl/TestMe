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
                              .HaveDependencyOnAny(new[] 
                              { 
                                "TestMe.TestCreation.Persistence",
                                "TestMe.TestCreation.App",
                                "TestMe.SharedKernel.Persistence",
                                "TestMe.SharedKernel.App",
                                "Microsoft.EntityFrameworkCore",
                              }) 
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
                             .HaveDependencyOnAny(new[]
                             {
                                 "TestMe.TestCreation.App",
                                 "TestMe.SharedKernel.App",
                                 "TestMe.SharedKernel.Domain",
                             }
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
                             .HaveDependencyOnAny(new[]
                             {
                                 "TestMe.SharedKernel.Persistence",
                                 "TestMe.SharedKernel.Domain",
                             }
                             )
                             .GetResult();
            Assert.IsTrue(result.IsSuccessful, "App has lost its independence!");
        }
    }
}
