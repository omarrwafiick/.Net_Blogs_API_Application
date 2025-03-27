using System.ComponentModel.DataAnnotations.Schema;

namespace BlogsAPI.Models
{
    public class CommentedBlogs : BaseEntity, IBlogProperty
    {
        public int BlogId { get; set; }
        public virtual Blog Blog { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }
        public DateTime TimeOccured { get; set; } = DateTime.Now;
        public string Comment { get; set; }
    }
}
