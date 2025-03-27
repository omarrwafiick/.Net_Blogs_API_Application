using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogsAPI.Models
{
    public class Contact : BaseEntity
    {
        [Column(TypeName = "nvarchar(350)")]
        public string Message { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
