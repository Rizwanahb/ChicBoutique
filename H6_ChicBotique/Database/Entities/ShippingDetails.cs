using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace H6_ChicBotique.Database.Entities
{
    public class ShippingDetails
    {
        
        [Key]
        public int Id { get; set; }  ////Unique Key for identifying each entity.
        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; } //ForeignKey for establishing the relationship between Order and ShippingDetails tables.

        public string Address { get; set; }
        public string City { get; set; }

        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }

        public Order Order { get; set; } ////Navigational object for getting Order table's data from this class

    }
}
