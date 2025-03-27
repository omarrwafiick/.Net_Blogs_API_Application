using System.Linq.Expressions;

namespace BlogsAPI.Repositories
{
    public class BlogCustomeRepository : IBlogCustomeRepository<Blog>
    {
        private readonly ApplicationDbContext _context; 
        public BlogCustomeRepository(ApplicationDbContext context)
        {
            _context = context; 
        }
        public IQueryable<Blog> GetPage(int skip, int take, Expression<Func<Blog, object>> include1, Expression<Func<Blog, object>> include2, Expression<Func<Blog, object>> include3)
        {
            return _context.Set<Blog>()
                    .AsNoTracking()
                    .Include(include1)
                    .Include(include2)
                    .Include(include3)
                    .Skip(skip * take)
                    .Take(take)
                    .AsQueryable();
        }
         
        public async Task<List<Blog>> GetAllAsync(Expression<Func<Blog, object>> include1, Expression<Func<Blog, object>> include2, Expression<Func<Blog, object>> include3)
        {
            return await _context.Set<Blog>()
                            .AsNoTracking()
                            .Include(include1)
                            .Include(include2)
                            .Include(include3)
                            .ToListAsync();
        }
    }
}
