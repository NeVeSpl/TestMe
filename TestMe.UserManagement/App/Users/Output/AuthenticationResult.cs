using System;
using System.Collections.Generic;
using System.Text;

namespace TestMe.UserManagement.App.Users.Output
{
    public sealed class AuthenticationResult
    {
        public bool IsAuthenticated { get; set; }
        public UserCredentialsDTO? UserCredentials { get; set; }

        public AuthenticationResult(UserCredentialsDTO userCredentials)
        {
            IsAuthenticated = true;
            UserCredentials = userCredentials;
        }

        public AuthenticationResult(bool isAuthenticated)
        {
            IsAuthenticated = isAuthenticated;
        }
    }
}
