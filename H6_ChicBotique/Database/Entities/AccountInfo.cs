
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace H6_ChicBotique.Database.Entities
{
    //This class is for holding the orders and Homeaddress of the client if they delete their profile from the website. 
    public class AccountInfo
    {
        [Key]
        public Guid Id { get; set; } // Unique identificator(Primary key) for AccountInfo table

        public DateTime CreatedDate { get; set; }

        public int? UserId { get; set; } //foreign key of user table for establshing
                                         //the relationaship between AccountInfo and User
        public IEnumerable<Order> Orders { get; set; } //navigation object
        public HomeAddress? HomeAddress { get; set; } //navigation object
        [ForeignKey("UserId")]
        public User? User { get; set; } //navigation object
    }
}

