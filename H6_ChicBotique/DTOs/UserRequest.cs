using System.ComponentModel.DataAnnotations;

namespace H6_ChicBotique.DTOs
{
    //Input from the clientside for making account, changing profile
    public class UserRequest 
    {
        [Required]
        [StringLength(128, ErrorMessage = "Email must be less than 128 chars")]
        public string Email { get; set; }

        [Required]
        [StringLength(32, ErrorMessage = "FirstName must be less than 32 chars")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(32, ErrorMessage = "LastName must be less than 32 chars")]
        public string LastName { get; set; }

        [Required]
        [StringLength(32, ErrorMessage = "Password must be less than 32 chars")]
        public string Password { get; set; }

    }
    public class UserRegisterRequest : UserRequest
    {
        [Required]
        [StringLength(32, ErrorMessage = "LastName must be less than 100 chars")]
        public string Address { get; set; }
        [Required]
        [StringLength(32, ErrorMessage = "LastName must be less than 100 chars")]
        public string City { get; set; }
        [Required]
        [StringLength(32, ErrorMessage = "LastName must be less than 100 chars")]
        public string PostalCode { get; set; }
        [Required]
        [StringLength(32, ErrorMessage = "LastName must be less than 100 chars")]
        public string Country { get; set; }
        [Required]
        [StringLength(32, ErrorMessage = "LastName must be less than 50 chars")]
        public string Telephone { get; set; }
    }
    public class UserUpdateRequest : UserRequest
    {
        [Required]
        [StringLength(32, ErrorMessage = "LastName must be less than 100 chars")]
        public string Address { get; set; }
        [Required]
        [StringLength(32, ErrorMessage = "LastName must be less than 100 chars")]
        public string City { get; set; }
        [Required]
        [StringLength(32, ErrorMessage = "LastName must be less than 100 chars")]
        public string PostalCode { get; set; }
        [Required]
        [StringLength(32, ErrorMessage = "LastName must be less than 100 chars")]
        public string Country { get; set; }
        [Required]
        [StringLength(32, ErrorMessage = "LastName must be less than 50 chars")]
        public string Telephone { get; set; }
    }



}
