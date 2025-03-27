using System.Linq.Expressions;

namespace BlogsAPI.Interfaces
{
    public interface IGetAllWithConditionRepository<T> : IScopedService<T> where T : class
    {
        IQueryable<T> GetAll(Expression<Func<T, bool>> condition);
    }
}
