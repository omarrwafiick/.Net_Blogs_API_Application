using System.ComponentModel.DataAnnotations.Schema;

namespace BlogsAPI.Models
{
    public class AppUser : IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "nvarchar(30)")]
        public string FullName { get; set; }  
        public bool Gender { get; set; } 
        public bool Notify { get; set; } = false;
        public virtual List<Blog>? Blogs { get; set; }
        public virtual List<Contact>? Contacts { get; set; }
        public virtual List<LikedBlogs>? LikedBlogsTbl { get; set; } 
        public virtual List<SharedBlogs>? SharedBlogsTbl { get; set; }
        public virtual List<CommentedBlogs>? CommentedBlogsTbl { get; set; }
    }
}
