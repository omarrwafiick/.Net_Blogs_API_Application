
using System.Linq.Expressions;

namespace BlogsAPI.Interfaces
{
    public interface IBlogCustomeRepository<T> : IScopedService<T> where T : class
    {
        Task<List<Blog>> GetAllAsync(Expression<Func<Blog, object>> include1, Expression<Func<Blog, object>> include2, Expression<Func<Blog, object>> include3);
        IQueryable<Blog> GetPage(int skip, int take, Expression<Func<Blog, object>> include1, Expression<Func<Blog, object>> include2, Expression<Func<Blog, object>> include3);
    }
}
