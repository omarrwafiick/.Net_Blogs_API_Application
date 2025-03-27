using System.ComponentModel.DataAnnotations.Schema;

namespace BlogsAPI.Models
{
    public class Blog : BaseEntity
    {
        [Column(TypeName = "nvarchar(30)")]
        public string Title { get; set; } 
        [Column(TypeName = "nvarchar(150)")]
        public string Description { get; set; } 
        [Column(TypeName = "nvarchar(400)")]
        public string Content { get; set; } 
        [Column(TypeName = "varbinary(max)")]
        public byte[] Image { get; set; } 
        public bool IsFeatured { get; set; }
        public int CategoryId { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public virtual Category Category { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }
        public virtual List<LikedBlogs>? LikedBlogsTbl { get; set; }
        public virtual List<SharedBlogs>? SharedBlogsTbl { get; set; }
        public virtual List<CommentedBlogs>? CommentedBlogsTbl { get; set; }
    }
}
