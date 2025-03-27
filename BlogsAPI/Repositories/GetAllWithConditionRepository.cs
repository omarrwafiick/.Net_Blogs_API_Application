using System.Linq.Expressions;

namespace BlogsAPI.Repositories
{
    public class GetAllWithConditionRepository<T> : IGetAllWithConditionRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        public GetAllWithConditionRepository(ApplicationDbContext context)
        {
            _context = context;
        } 
        public IQueryable<T> GetAll(Expression<Func<T, bool>> condition)
        {
            return _context.Set<T>().AsNoTracking().Where(condition).AsQueryable();
        }
    }
}
