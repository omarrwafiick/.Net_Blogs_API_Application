
namespace BlogsAPI.Repositories
{
    public class CommonRepository<T> : ICommonRepository<T> where T : class, IBaseEntity
    {
        private readonly ApplicationDbContext _context; 
        public CommonRepository(ApplicationDbContext context)
        {
            _context = context; 
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>()
                            .AsNoTracking()
                            .ToListAsync();
        } 
        
        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>()
                            .Where(x => x.Id == id)
                            .SingleOrDefaultAsync();
        }

        public async Task<string?> AddAsync(T model)
        {
            await _context.Set<T>().AddAsync(model);
            var result = await _context.SaveChangesAsync();
            if (result <= 0) 
                return null;
            return "added successfully";
        }

        public async Task<string?> UpdateAsync(T model)
        {
            await Task.Run(()=> _context.Set<T>().Update(model));
            var result = await _context.SaveChangesAsync();
            if (result <= 0)
                return null;
            return "updated successfully";
        }

        public async Task<string?> DeleteAsync(T model)
        {
            await Task.Run(() => _context.Set<T>().Remove(model));
            var result = await _context.SaveChangesAsync();
            if (result <= 0)
                return null;
            return "deleted successfully";
        }


    }
}
