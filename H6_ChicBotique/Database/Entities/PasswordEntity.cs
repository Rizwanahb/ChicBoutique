
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace H6_ChicBotique.Database.Entities
{
    public class PasswordEntity

    {
        [Key]
        public int PasswordId { get; set; } //Unique Key for identifying each entity.
        [ForeignKey("User")]
        public int UserId { get; set; } //ForeignKey for establishing the relationship between PasswordEntity and User tables.
        [Column(TypeName = "nvarchar(128)")]
        public string Password { get; set; }
        [Column(TypeName = "nvarchar(64)")]
        public string Salt { get; set; }
        [Column(TypeName = "date")]
        public DateTime LastUpdated { get; set; }

        public User User { get; set; } ////Navigational object for getting User table's data from this class
    }
}
