using System;
using System.Runtime.CompilerServices;
using TestMe.UserManagement.Domain;
using TestMe.UserManagement.Persistence;

[assembly: InternalsVisibleTo("TestMe.Presentation.API.Tests")]
[assembly: InternalsVisibleTo("TestMe.UserManagement.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]      // needed by Moq
namespace TestMe.UserManagement
{
    internal static class TestUtils
    {
        public const long   ValidUser1Id = 1;
        public const String ValidUser1Mail = "a@a.al";
        public const String ValidUser1Password = "aaaaAAAA";        
        public const UserRole ValidUser1Role = UserRole.Regular;

        public const long ValidUser2Id = 2;
        public const String ValidUser2Mail = "b@b.com";
        public const String ValidUser2Password = "bbbbBBBB";        
        public const UserRole ValidUser2Role = UserRole.Admin;

        public static void Seed(UserManagementDbContext context)
        {
            AddUser(context, ValidUser1Mail, ValidUser1Password, "Abigail", ValidUser1Role);
            AddUser(context, ValidUser2Mail, ValidUser2Password, "Bethany", ValidUser2Role);
            AddUser(context, "c.c@c.eu", "1a5g9j2k", "Chloe", UserRole.Regular);
            AddUser(context, "u1978u@k.k.uk", "kd94j7djdc83j", "Daisy", UserRole.Regular);
        }

        private static void AddUser(UserManagementDbContext context, string email, string password, string name, UserRole role)
        {
            var user = new User(EmailAddress.Create(email), Password.Create(password)) { Name = name, Role = role };
            context.Users.Add(user);
            context.SaveChanges();
        }
    }
}
