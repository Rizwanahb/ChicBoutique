using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace H6_ChicBotique.Database.Entities
{
    public class Payment
    {
        [Key]
        public int Id { get; set; } //Unique Key for identifying each entity.

        [Column(TypeName = "nvarchar(50)")]
        public string? Status { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string? TransactionId { get; set; }
        [Column(TypeName ="Decimal(10, 03)")]
        public decimal  Amount { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string? PaymentMethod { get; set; }
        public DateTime? TimePaid { get; set; }
        public Order Order { get; set; } //Navigational object for getting Order table's data from this class
    }
}
