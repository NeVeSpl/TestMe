using TestMe.UserManagement.Domain;

namespace TestMe.UserManagement.Persistence
{
    public static class TestUtils
    {
        public static void Seed(UserManagementDbContext context)
        {
            context.Users.Add(new User() { Name = "User A" });
            context.Users.Add(new User() { Name = "User B" });
            
            context.SaveChanges();
        }
    }
}
