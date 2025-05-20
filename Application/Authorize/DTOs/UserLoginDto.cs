namespace Application.Authorize.DTOs
{
    public class UserLoginDto
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}
