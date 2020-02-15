using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestMe.BuildingBlocks.Domain;
using TestMe.BuildingBlocks.Tests;
using TestMe.UserManagement.App;
using TestMe.UserManagement.App.Users;
using TestMe.UserManagement.App.Users.Input;
using TestMe.UserManagement.App.Users.Output;
using TestMe.UserManagement.Domain;
using TestMe.UserManagement.Persistence;

namespace TestMe.UserManagement.Tests
{
    [TestClass]
    public class UsersServiceTests : BaseFixture
    {
        private UserManagementDbContext userManagementDbContext;
        private UsersService serviceUnderTest;

        private protected override FakeDatabaseType GetDatabaseType()
        {
            return FakeDatabaseType.SQLiteInMemory;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            userManagementDbContext = CreateUserManagementDbContext();

            var correlationIdProviderMock = new Mock<ITraceIdProvider>();
            correlationIdProviderMock.Setup(x => x.TraceId).Returns(() => "666");

            serviceUnderTest = new UsersService(userManagementDbContext, correlationIdProviderMock.Object, CreateAutoMapper());
        }

        [TestCleanup]
        public void TestCleanup()
        {
            userManagementDbContext.Dispose();
        }

        [TestMethod]
        public void UserCannotBeCreatedWithEmailAddressThatAlreadyExists()
        {
            var command = new CreateUser()
            {
                EmailAddress = TestUtils.ValidUser1Mail,
                Password = "12345678",
                Name = "yes",
            };            

            Assert.ThrowsException<DomainException>(() => serviceUnderTest.CreateUser(command), DomainExceptions.User_with_given_email_address_already_exists);
        }

        [TestMethod]
        [DataRow("a@a")]
        [DataRow("123")]
        public void UserCannotBeCreatedWithInvalidEmailAddress(string invalidEmail)
        {
            var command = new CreateUser()
            {
                EmailAddress = invalidEmail,
                Password = "12345678",
                Name = "yes",
            };

            Assert.ThrowsException<DomainException>(() => serviceUnderTest.CreateUser(command), DomainExceptions.Email_address_is_invalid);
        }

        [TestMethod]
        [DataRow("p.s@z.pl")]        
        public void UserCanBeCreatedWithTrickyValidEmailAddress(string validEmail)
        {
            var command = new CreateUser()
            {
                EmailAddress = validEmail,
                Password = "12345678",
                Name = "yes",
            };

            long createdUserId = serviceUnderTest.CreateUser(command);

            using (var context = CreateUserManagementDbContext())
            {
                User user = context.Users.Find(createdUserId);
                Assert.AreEqual(validEmail, user.EmailAddress.Value);
            }
        }

        [TestMethod]
        [DataRow(TestUtils.ValidUser1Mail+".", TestUtils.ValidUser1Password)]
        [DataRow(TestUtils.ValidUser2Mail, " "+TestUtils.ValidUser2Password)]
        [DataRow(TestUtils.ValidUser1Mail, TestUtils.ValidUser2Password)]
        public void UserWithInvalidCredentialIsVerifiedNegatively(string email, string password)
        {
            var command = new LoginUser()
            {
                Email = email,
                Password = password
            };
            AuthenticationResult credentials = serviceUnderTest.VerifyUserCredentials(command);
            Assert.IsFalse(credentials.IsAuthenticated);
        }
    }
}