namespace IqOption.BackEnd.DTO
{
    public class LoginIqOptionDTO
    {
        public LoginIqOptionDTO(string email, string password)
        {
            Email = email;
            Password = password;
        }
        public string Email { get; }
        public string Password { get; }
    }
}
