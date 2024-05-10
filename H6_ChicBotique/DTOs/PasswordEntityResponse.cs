using System.ComponentModel.DataAnnotations;

namespace H6_ChicBotique.DTOs
{
    public class PasswordEntityResponse
    {
        //It is a output for the corresponding request
        public string Password { get; set; }

        [Required]
        [StringLength(32, ErrorMessage = "FirstName must be less than 32 chars")]
        public string Salt { get; set; }

        [Required]
        public DateTime LastUpdatedDate { get; set; }
    }
}
