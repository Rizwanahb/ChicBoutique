using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace H6_ChicBotique.Database.Entities
{
    public class OrderDetails
    {
        [Key]
        public int Id { get; set; } //Unique Key for identifying each entity.
        public int ProductId { get; set; }

        [Column(TypeName = "nvarchar(32)")] //this is a columnAttribute from System.CoponentModel.DataAnnotations (defined in enityframework.dll)
        public string ProductTitle { get; set; }

        [Column(TypeName = "decimal(6,2)")]
        public decimal ProductPrice { get; set; }


        [Column(TypeName = "smallint")]
        public int Quantity { get; set; }

      


        public int OrderId { get; set; } //ForeignKey for establishing the relationship between OrderDetails and Order tables.

        [ForeignKey("OrderId")]
        public Order Order { get; set; }  ////Navigation property for getting Order table's data from this class

        [ForeignKey("ProductId")] //ForeignKey for establishing the relationship between OrderDetails and Product tables.

        public Product Product { get; set; } ////Navigation property for getting Product table's data from this class
    }
}
