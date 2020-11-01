using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TestMe.BuildingBlocks.App;
using TestMe.BuildingBlocks.Domain;
using TestMe.UserManagement.App.Users.Input;
using TestMe.UserManagement.App.Users.Output;
using TestMe.UserManagement.Domain;
using TestMe.UserManagement.Persistence;

namespace TestMe.UserManagement.App.Users
{
    public class UsersService
    {
        private readonly UserManagementDbContext context;
        private readonly ITraceIdProvider correlationIdProvider;
        private readonly IConfigurationProvider mapperConfiguration;

        public UsersService(UserManagementDbContext context, ITraceIdProvider correlationIdProvider, IConfigurationProvider mapperConfiguration)
        {
            this.context = context;
            this.correlationIdProvider = correlationIdProvider;
            this.mapperConfiguration = mapperConfiguration;
        }

        
        /// <returns>null if credentials were verified negatively</returns>
        public AuthenticationResult VerifyUserCredentials(LoginUser credentials)
        {
            var user = context.Users.AsNoTracking().FirstOrDefault(x => x.EmailAddress.Value == credentials.Email);
            if ((user != null) && (user.Password.VerifyPassword(credentials.Password)))
            {
                return new AuthenticationResult(new UserCredentialsDTO(user));
            }
            return new AuthenticationResult(false);
        }  
        public async Task<AuthenticationResult> VerifyUserCredentialsAsync(LoginUser credentials)
        {
            var user = await context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.EmailAddress.Value == credentials.Email);
            if ((user != null) && (user.Password.VerifyPassword(credentials.Password)))
            {
                return new AuthenticationResult(new UserCredentialsDTO(user));
            }
            return new AuthenticationResult(false);
        }

        public async Task<bool> IsEmailAddressTaken(string emailAddress)
        {
            return await context.Users.AsNoTracking().AnyAsync(x => x.EmailAddress.Value == emailAddress);
        }

        public async Task<long> CreateUser(CreateUser createUser)
        {
            bool emailIsTaken = await IsEmailAddressTaken(createUser.EmailAddress);
            if (emailIsTaken)
            {
                throw new DomainException(DomainExceptions.User_with_given_email_address_already_exists);
            }

            User? createdUser = null;
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                createdUser = new User(EmailAddress.Create(createUser.EmailAddress), Password.Create(createUser.Password));
                createdUser.Name = createUser.Name;
                context.Users.Add(createdUser);
                await context.SaveChangesAsync();

                context.AddEvent(createdUser.CreateEvent(), correlationIdProvider.TraceId);
                await context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            return createdUser!.UserId;
        }


        public async Task<CursorPagedResults<UserDTO>> GetUsers(ReadUsers readUsers)
        {
            var result = new CursorPagedResults<UserDTO>();

            if (readUsers.Pagination.FetchNext > 0)
            {
                result.Result = await context.Users.AsNoTracking().Where(x => x.UserId > readUsers.Pagination.Cursor).Take(readUsers.Pagination.FetchNext).ProjectTo<UserDTO>(mapperConfiguration).ToListAsync();
                result.Cursor = readUsers.Pagination.Cursor;
                result.NextCursor = result.Result.LastOrDefault()?.UserId;
            }
            else
            {
                var users = await context.Users.AsNoTracking().Where(x => x.UserId <= readUsers.Pagination.Cursor).TakeLast(readUsers.Pagination.FetchNext - 1).ProjectTo<UserDTO>(mapperConfiguration).ToListAsync();
                if (users.Count() != Math.Abs(readUsers.Pagination.FetchNext))
                {
                    result.Result = users;
                    result.Cursor = 0;
                }
                else
                {
                    result.Result = users.Skip(1).ToList();
                    result.Cursor = result.Result.FirstOrDefault()?.UserId;
                }
                result.NextCursor = readUsers.Pagination.Cursor;
            }

            return result;
        }
    }
}