namespace BlogsAPI.Interfaces
{
    public interface IMethodScope
    {
        public int BlogId { get; set; }
        public string AppUserId { get; set; }
        public DateTime TimeOccured { get; set; }
    }
}
