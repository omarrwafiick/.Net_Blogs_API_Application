using System.ComponentModel.DataAnnotations.Schema; 

namespace BlogsAPI.Models
{
    public class Category : BaseEntity
    {
        [Column(TypeName = "nvarchar(10)")]
        public string Name { get; set; } 
        public virtual List<Blog>? Blog { get; set; }
    }
}
