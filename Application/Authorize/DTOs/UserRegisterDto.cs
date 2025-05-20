namespace Application.Authorize.DTOs
{
    public class UserRegisterDto
    {
        public required string UserName { get; set; }
        public required string UserEmail { get; set; }
        public required string Password { get; set; }
    }
}