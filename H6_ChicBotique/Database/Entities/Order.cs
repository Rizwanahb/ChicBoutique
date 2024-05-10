
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace H6_ChicBotique.Database.Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; } //Unique Key for identifying each entity.

        [Column(TypeName = "Date")]  //this is a columnAttribute from System.ComponentModel.DataAnnotations (defined in enityframework.dll)
        public DateTime OrderDate { get; set; }

        [ForeignKey("AccountInfoId")]
        public Guid AccountInfoId { get; set; }
        public AccountInfo AccountInfo { get; set; }
        public ShippingDetails ShippingDetails { get; set; }

        public List<OrderDetails> OrderDetails { get; set; } = new();

        [ForeignKey("PaymentId")]  //ForeignKey for establishing the relationship between Order and Payment tables.
        
        public int PaymentId { get; set; }
        public Payment Payment { get; set; }  ////Navigation property for getting Payment  tables data from this class
    }

}
