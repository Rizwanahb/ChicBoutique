using System.ComponentModel.DataAnnotations;

namespace H6_ChicBotique.DTOs
{
    //Input from the clientside 
    public class PasswordEntityRequest
    {
        [Required]
        [StringLength(128, ErrorMessage = "Email must be less than 128 chars")]
        public string Password { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
