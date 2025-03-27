
namespace BlogsAPI.Interfaces
{
    public interface ICommonRepository<T> : IScopedService<T> where T : class , IBaseEntity
    {
        Task<List<T>> GetAllAsync(); 
        Task<T> GetByIdAsync(int id);
        Task<string?> AddAsync(T model);
        Task<string?> UpdateAsync(T model);
        Task<string?> DeleteAsync(T model);
    }
}
