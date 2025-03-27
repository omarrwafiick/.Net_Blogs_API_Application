 
namespace BlogsAPI.Interfaces
{
    public interface IBlogsService<T> : IScopedService<T> where T : class
    {
        Task<List<Blog>?> GetAllBlogsAsync();
        Task<List<Blog>?> GetAllFeaturesBlogsAsync();
        Task<List<Blog>?> GetBlogsPageAsync(int skip = 0,  int take = 10);
        Task<Blog?> GetBlogByIdAsync(int Id);
        Task<List<Blog>?> GetUserBlogsAsync(string userId);
        Task<List<Category>> GetAllCategoryAsync();
        Task<ServiceResult> CreateBlogAsync(CreateBlogDto dto);
        Task<ServiceResult> UpdateBlogAsync(string userId, int Id, UpdateBlogDto dto);
        Task<ServiceResult> AddLikeAsync(int blogId, string userId);
        Task<ServiceResult> AddShareAsync(int blogId, string userId);
        Task<ServiceResult> AddCommentAsync(int blogId, string userId, string comment);
        Task<ServiceResult> RemoveBlogPropertyAsync<T>(int blogId, string userId, IGetAllWithConditionRepository<T> customeRepository, ICommonRepository<T> mainRepository) where T : class, IBlogProperty;
        Task<ServiceResult> IsRelatedToUserAsync<T>(int blogId, string userId, IGetAllWithConditionRepository<T> mainRepository) where T : class, IBlogProperty;
        Task<ServiceResult> DeleteBlogAsync(int Id);
    }
}
