namespace BlogsAPI.Interfaces
{
    public interface ICustomeDeleteRangeRepository<T> : IScopedService<T> where T : class
    {
        Task<string?> DeleteAsync(List<T> models);
    }
}
