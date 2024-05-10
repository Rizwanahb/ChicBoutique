
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace H6_ChicBotique.Database.Entities
{
    //This is for saving HomeAddress of the Customers
    public class HomeAddress
    {
        [Key]
        public int Id { get; set; } //Unique Key for identifying each entity.

        public string Address { get; set; }
        public string City { get; set; }

        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string TelePhone { get; set; }

        [ForeignKey("AccountInfoId")]
        public Guid AccountInfoId { get; set; }  //ForeignKey of the AccountInfo table
                                                 //for establishing the relationship between HomeAddress and AccountInfo tables.
        /// </summary>
        public AccountInfo AccountInfo { get; set; } //Navigation property for getting AccountInfo tables data from this class
    }
}
