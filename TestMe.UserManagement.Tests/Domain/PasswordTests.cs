using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestMe.UserManagement.Domain;

namespace TestMe.UserManagement.Tests.Domain
{
    [TestClass]
    public class PasswordTests
    {
        [TestMethod]
        [DataRow("Testtest")]
        [DataRow("12341234")]
        public void CreatedPasswordIsVerifiedPositively(string password)
        {
            Password createdPassword = Password.Create(password);
            bool isPasswordCorrect = createdPassword.VerifyPassword(password);
            Assert.IsTrue(isPasswordCorrect);
        }
    }
}
