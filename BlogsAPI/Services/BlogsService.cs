
using BlogsAPI.Models;
using Microsoft.AspNetCore.Identity;

public class BlogService : IBlogsService<Blog>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ICommonRepository<Blog> _blogRepository;  
    private readonly ICommonRepository<LikedBlogs> _likedBlogsCommonRepo;
    private readonly ICommonRepository<SharedBlogs> _sharedBlogsCommonRepo;
    private readonly ICommonRepository<CommentedBlogs> _commentedBlogsCommonRepo;
    private readonly ICommonRepository<Category> _categoriesCommonRepo;
    private readonly IBlogCustomeRepository<Blog> _blogsCustomeRepo;
    private readonly IGetAllWithConditionRepository<Blog> _blogsGetAllWithConditionCustomeRepo;
    private readonly IGetByIdWithConditionCustomeRepository<Blog> _blogsGetByIdWithConditionCustomeRepo;
    private readonly IGetAllWithConditionRepository<AppUser> _usersGetAllWithConditionCustomeRepo;
    private readonly IGetAllWithConditionRepository<LikedBlogs> _likedBlogsGetAllWithConditionCustomeRepo;
    private readonly IGetAllWithConditionRepository<SharedBlogs> _sharedBlogsGetAllWithConditionCustomeRepo;
    private readonly IGetAllWithConditionRepository<CommentedBlogs> _commentedBlogsGetAllWithConditionCustomeRepo;
    public BlogService(
        UserManager<AppUser> userManager,
        ICommonRepository<Blog> blogRepository,  
        ICommonRepository<LikedBlogs> likedBlogsCommonRepo,
        ICommonRepository<SharedBlogs> sharedBlogsCommonRepo,
        ICommonRepository<CommentedBlogs> commentedBlogsCommonRepo,
        ICommonRepository<Category> categoriesCommonRepo,
        IBlogCustomeRepository<Blog> blogsCustomeRepo,
        IGetAllWithConditionRepository<Blog> blogsGetAllWithConditionCustomeRepo,
        IGetByIdWithConditionCustomeRepository<Blog> blogsGetByIdWithConditionCustomeRepo,
        IGetAllWithConditionRepository<AppUser> usersGetAllWithConditionCustomeRepo,
        IGetAllWithConditionRepository<LikedBlogs> likedBlogsGetAllWithConditionCustomeRepo,
        IGetAllWithConditionRepository<SharedBlogs> sharedBlogsGetAllWithConditionCustomeRepo,
        IGetAllWithConditionRepository<CommentedBlogs> commentedBlogsGetAllWithConditionCustomeRepo
        )
    {
        _userManager = userManager;
        _blogRepository = blogRepository;  
        _likedBlogsCommonRepo = likedBlogsCommonRepo;
        _sharedBlogsCommonRepo = sharedBlogsCommonRepo;
        _commentedBlogsCommonRepo = commentedBlogsCommonRepo;
        _categoriesCommonRepo = categoriesCommonRepo;
        _blogsCustomeRepo = blogsCustomeRepo;
        _blogsGetAllWithConditionCustomeRepo = blogsGetAllWithConditionCustomeRepo;
        _blogsGetByIdWithConditionCustomeRepo = blogsGetByIdWithConditionCustomeRepo;
        _usersGetAllWithConditionCustomeRepo = usersGetAllWithConditionCustomeRepo;
        _likedBlogsGetAllWithConditionCustomeRepo = likedBlogsGetAllWithConditionCustomeRepo;
        _sharedBlogsGetAllWithConditionCustomeRepo = sharedBlogsGetAllWithConditionCustomeRepo;
        _commentedBlogsGetAllWithConditionCustomeRepo = commentedBlogsGetAllWithConditionCustomeRepo;
    }
    public async Task<List<Blog>?> GetAllBlogsAsync()
    {
        var result = await _blogsCustomeRepo.GetAllAsync(x => x.CommentedBlogsTbl, x => x.LikedBlogsTbl, x => x.SharedBlogsTbl);

        if (result is not null)
            return result;

        return null;
    }

    public async Task<List<Blog>?> GetAllFeaturesBlogsAsync()
    { 
        var result = _blogsGetAllWithConditionCustomeRepo.GetAll(x => x.IsFeatured);

        if (result is not null)
            return result.ToList();

        return null;
    }

    public async Task<List<Blog>?> GetBlogsPageAsync(int skip, int take)
    {  
        var result = _blogsCustomeRepo.GetPage(skip, take, x => x.CommentedBlogsTbl, x => x.LikedBlogsTbl, x => x.SharedBlogsTbl).ToList();
          
        return result is not null ? result : null;
    }

    public async Task<Blog?> GetBlogByIdAsync(int Id)
    {
        var result = await _blogsGetByIdWithConditionCustomeRepo.GetByIdAsync(Id, x => x.CommentedBlogsTbl, x => x.LikedBlogsTbl, x => x.SharedBlogsTbl);
         
        return result is not null ? result : null;
    }

    public async Task<List<Blog>?> GetUserBlogsAsync(string userId)
    { 
        var userBlogs = _blogsGetAllWithConditionCustomeRepo.GetAll(x => x.AppUserId == userId);

        if (userBlogs is null)
            return null;

        return userBlogs.ToList();
    } 

    public async Task<List<Category>?> GetAllCategoryAsync()
    { 
        var result = await _categoriesCommonRepo.GetAllAsync();

        if (result is not null)
            return result;

        return null;
    }

    public async Task<ServiceResult> CreateBlogAsync(CreateBlogDto dto)
    {
        var user = await _userManager.FindByIdAsync(dto.UserId);

        if (user is null)
            return ServiceResult.Failure($"User not found with ID {dto.UserId}");

        if (dto.IsFeatured)
        {
            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains(AppConstants.ADMIN))
                dto.IsFeatured = false;
        }

        if (!HelperMethods.ValidateImage(dto.Image))
            return ServiceResult.Failure($"Invalid image extension or size. Allowed: {AppConstants.ALLOWEDEXTENSIONS}, Max: {AppConstants.MAXALLOWEDIMAGESIZE} bytes");

        var blog = dto.FromBlogCreateDto(); 

        var result = await _blogRepository.AddAsync(blog);

        if (result is null)
            return ServiceResult.Failure("Something went wrong while creating a new blog instance"); 

        return ServiceResult.Success("Blog created successfully");
    }

    public async Task<ServiceResult> UpdateBlogAsync(string userId, int Id, UpdateBlogDto dto)
    {
        var user = await _userManager.FindByIdAsync(userId);

        var blog = await _blogRepository.GetByIdAsync(Id);

        if (user is null || blog is null)
            return ServiceResult.Failure($"User or blog was not found using this id/s {userId}/{Id}");

        if (dto.Image is not null && !HelperMethods.ValidateImage(dto.Image))
            return ServiceResult.Failure($"image extension or size is not valid only {AppConstants.ALLOWEDEXTENSIONS} are allowed with maximum length of {AppConstants.MAXALLOWEDIMAGESIZE} bytes/15MB");

        var finalBlog = dto.FromUpdateBlogToBlog(blog);

        var result = await _blogRepository.UpdateAsync(finalBlog);

        if (result is not null)
            return ServiceResult.Success();

        return ServiceResult.Failure("Something went wrong while updating a blog instance");
    }

    public async Task<ServiceResult> AddLikeAsync(int blogId, string userId)
    {
        var checkExistenceResult = await CheckBlogWithUser(blogId, userId);

        if (checkExistenceResult is not null)
            return ServiceResult.Failure(checkExistenceResult); 

        var exist = _likedBlogsGetAllWithConditionCustomeRepo.GetAll(x => x.BlogId == blogId && x.AppUserId == userId);

        if (exist.Any())
            return ServiceResult.Failure($"This blog is liked already by this user of id {userId}");
          
        var newLikedBlog = new LikedBlogs { AppUserId = userId, BlogId = blogId };

        var addingLike = await _likedBlogsCommonRepo.AddAsync(newLikedBlog);

        if (addingLike is null)
            return ServiceResult.Failure("Something went wrong while adding a like blog instance");

        return ServiceResult.Success("Blog is liked successfully");
    }

    public async Task<ServiceResult> AddShareAsync(int blogId, string userId)
    {
        var checkExistenceResult = await CheckBlogWithUser(blogId, userId);

        if (checkExistenceResult is not null)
            return ServiceResult.Failure(checkExistenceResult);

        var exist = _sharedBlogsGetAllWithConditionCustomeRepo.GetAll(x => x.BlogId == blogId && x.AppUserId == userId);

        if (exist.Any())
            return ServiceResult.Failure($"This blog is shared already by this user of id {userId}"); 

        var newSharedBlog = new SharedBlogs { AppUserId = userId, BlogId = blogId };

        var addingLike = await _sharedBlogsCommonRepo.AddAsync(newSharedBlog);

        if (addingLike is null)
            return ServiceResult.Failure("Something went wrong while adding a share blog instance");

        return ServiceResult.Success("Blog is shared successfully");
    }

    public async Task<ServiceResult> AddCommentAsync(int blogId, string userId, string comment)
    {
        var checkExistenceResult = await CheckBlogWithUser(blogId, userId);

        if (checkExistenceResult is not null)
            return ServiceResult.Failure(checkExistenceResult);

        var san = new HtmlSanitizer();

        comment = san.Sanitize(comment);

        var newCommentedBlog = new CommentedBlogs { AppUserId = userId, BlogId = blogId, Comment = comment };

        var addingLike = await _commentedBlogsCommonRepo.AddAsync(newCommentedBlog);

        if (addingLike is null)
            return ServiceResult.Failure("Something went wrong while adding a comment blog instance");

        return ServiceResult.Success("Blog is commented successfully");
    }

    public async Task<ServiceResult> RemoveBlogPropertyAsync<T>(int blogId, string userId, IGetAllWithConditionRepository<T> customeRepository, ICommonRepository<T> mainRepository) where T : class, IBlogProperty
    {
        var checkExistenceResult = await CheckBlogWithUser(blogId, userId);

        if (checkExistenceResult is not null)
            return ServiceResult.Failure(checkExistenceResult);

        var blogProperty = customeRepository.GetAll(x => x.BlogId == blogId && x.AppUserId == userId);

        if (blogProperty is null)
            return ServiceResult.Failure($"No related data was found to this user");

        var removingProperty = await mainRepository.DeleteAsync(blogProperty.FirstOrDefault());

        if (removingProperty is null)
            return ServiceResult.Failure("Something went wrong while removing a blog property");

        return ServiceResult.Success("Operation is finished successfully");
    }

    public async Task<ServiceResult> IsRelatedToUserAsync<T>(int blogId, string userId, IGetAllWithConditionRepository<T> mainRepository) where T : class, IBlogProperty
    {
        var checkExistenceResult = await CheckBlogWithUser(blogId, userId);

        if (checkExistenceResult is not null)
            return ServiceResult.Failure(checkExistenceResult);

        var isRelated = mainRepository.GetAll(x=>x.BlogId == blogId&& x.AppUserId == userId);

        if (!isRelated.Any())
            return ServiceResult.Failure($"This Property is not related to this user of id {userId}"); 

        return ServiceResult.Success($"Blog property is related to this user of id {userId}");
    }

    public async Task<ServiceResult> DeleteBlogAsync(int Id)
    {
        var blog = await _blogRepository.GetByIdAsync(Id);

        if (blog is null)
            return ServiceResult.Failure($"Blog was not found by this id {Id}");

        var result = await _blogRepository.DeleteAsync(blog);

        if (result is not null)
            return ServiceResult.Success();

        return ServiceResult.Failure("Something went wrong while deleting a blog instance");
    }

    public async Task<string?> CheckBlogWithUser(int blogId, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        var blog = await _blogRepository.GetByIdAsync(blogId);

        return user is null || blog is null ? $"User or blog was not found using this id/s {userId}/{blogId}" : null;  
    }
     
}
