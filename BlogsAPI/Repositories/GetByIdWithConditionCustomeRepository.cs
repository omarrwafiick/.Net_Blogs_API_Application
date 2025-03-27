 using System.Linq.Expressions;

namespace BlogsAPI.Repositories
{
    public class GetByIdWithConditionCustomeRepository<T> : IGetByIdWithConditionCustomeRepository<T> where T : class, IBaseEntity
    {
        private readonly ApplicationDbContext _context; 
        public GetByIdWithConditionCustomeRepository(ApplicationDbContext context)
        {
            _context = context; 
        }
        public async Task<T> GetByIdAsync(int id, Expression<Func<T, object>> include1, Expression<Func<T, object>> include2, Expression<Func<T, object>> include3)
        {
            return await _context.Set<T>() 
                            .AsNoTracking()
                            .Include(include1)
                            .Include(include2)
                            .Include(include3)
                            .Where(x => x.Id == id)
                            .SingleOrDefaultAsync();
        }
    }
}
