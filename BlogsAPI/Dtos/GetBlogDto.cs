namespace BlogsAPI.Dtos
{
    public class GetBlogDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public bool IsFeatured { get; set; }
        public int CategoryId { get; set; }
        public string Gender { get; set; }
        public List<string> SharedBy { get; set; }
        public List<string> LikedBy { get; set; }
        public List<GetCommentedDto> CommentsInfo { get; set; }
        public DateTime CreatedAt { get; set; }  
    }
}
