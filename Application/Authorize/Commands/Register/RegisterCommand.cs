using Domain.Common;
using MediatR;

namespace Application.Authorize.Commands.Register
{
    public class RegisterCommand : IRequest<OperationResult<string>>
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Password { get; set; }

        public RegisterCommand(string userName, string email, string password) 
        { 
            UserName = userName;
            UserEmail = email;
            Password = password;            
        }
    }
}
