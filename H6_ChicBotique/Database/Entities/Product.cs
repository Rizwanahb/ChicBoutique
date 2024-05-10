using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace H6_ChicBotique.Database.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; } //Unique Key for identifying each entity.

        [Column(TypeName = "nvarchar(32)")]
        public string Title { get; set; }

        [Column(TypeName = "decimal(6,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "nvarchar(32)")]
        public string Description { get; set; }

        [Column(TypeName = "nvarchar(32)")]
        public string Image { get; set; }

        [Column(TypeName = "smallint")]
        public int Stock { get; set; }


        public int? CategoryId { get; set; } //ForeignKey for establishing the relationship between Category and Product tables.

        [ForeignKey("CategoryId")]
        public Category Category { get; set; } ////Navigational object for getting Category table's data from this class
    }

    
}