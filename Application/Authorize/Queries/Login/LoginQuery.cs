using Domain.Common;
using MediatR;

namespace Application.Authorize.Queries.Login
{
    public class LoginQuery : IRequest<OperationResult<string>>
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public LoginQuery(string username, string password) 
        { 
            UserName = username;
            Password = password;
        }
    }
}
