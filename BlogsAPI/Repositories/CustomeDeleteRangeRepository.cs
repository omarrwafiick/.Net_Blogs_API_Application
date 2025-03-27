
using Microsoft.EntityFrameworkCore;

namespace BlogsAPI.Repositories
{
    public class CustomeDeleteRangeRepository<T> : ICustomeDeleteRangeRepository<T> where T : class, IBaseEntity
    {
        private readonly ApplicationDbContext _context;
        public CustomeDeleteRangeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string?> DeleteAsync(List<T> models)
        {
            await Task.Run(() => _context.Set<T>().RemoveRange(models));
            var result = await _context.SaveChangesAsync();
            if (result <= 0)
                return null;
            return "deleted successfully";
        }
    }
}
