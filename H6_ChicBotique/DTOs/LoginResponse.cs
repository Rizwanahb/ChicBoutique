using H6_ChicBotique.Helpers;

namespace H6_ChicBotique.DTOs
{
    public class LoginResponse
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }

        public Role Role { get; set; }
        public string Token { get; set; }
    }
}
