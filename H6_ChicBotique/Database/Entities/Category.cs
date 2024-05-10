using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace H6_ChicBotique.Database.Entities
{
    public class Category
    {
            // Unique identificator(Primary key) for category table
            [Key]
            public int Id { get; set; }

            // Name of the Category with a maximum length 20
            [Column(TypeName = "nvarchar(20)")]
            public string? CategoryName { get; set; }

            // Navigation for the list of products for one Cataegory which has been initialized as empty list.
            public List<Product> Products { get; set; } = new();
        }
    }
