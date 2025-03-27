using System.Linq.Expressions;

namespace BlogsAPI.Interfaces
{
    public interface IGetByIdWithConditionCustomeRepository<T> : IScopedService<T> where T : class
    {
        Task<T> GetByIdAsync(int id, Expression<Func<T, object>> include1, Expression<Func<T, object>> include2, Expression<Func<T, object>> include3);
    }
}
