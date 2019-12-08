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
        public void DomainIsNotAccessibleFromOutside()
        {
            var result = Types.InAssembly(TestCreationAssembly)
                              .That()
                              .ResideInNamespace("TestMe.TestCreation.Domain")
                              .Should()
                              .NotBePublic()
                              .GetResult();
            Assert.IsTrue(result.IsSuccessful);
        }

        /// <summary>
        /// This test fails because of a bug in NetArchTest
        /// </summary>
        [TestMethod]
        public void PersistenceIsNotAccessibleFromOutside()
        {
            var result = Types.InAssembly(TestCreationAssembly)
                              .That()
                              .ResideInNamespace("TestMe.TestCreation.Persistence")                             
                              .Should()
                              .NotBePublic()                             
                              .GetResult();
            Assert.IsTrue(result.IsSuccessful);
        }
       
        [TestMethod]
        [DataRow("Service")]
        [DataRow("Readers")]
        public void AppServicesAndReadersAreNotAccessibleFromOutside(string end)
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
    }
}