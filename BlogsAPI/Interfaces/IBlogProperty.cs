namespace BlogsAPI.Interfaces
{
    public interface IBlogProperty : IBaseEntity
    {
        public int BlogId { get; set; }
        public Blog Blog { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
