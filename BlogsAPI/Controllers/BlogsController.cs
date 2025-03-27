using Microsoft.AspNetCore.Mvc;

namespace BlogsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    { 
        private readonly ICommonRepository<LikedBlogs> _likedBlogsCommonRepo;
        private readonly ICommonRepository<SharedBlogs> _sharedBlogsCommonRepo;
        private readonly ICommonRepository<CommentedBlogs> _commentedBlogsCommonRepo;
        private readonly IGetAllWithConditionRepository<LikedBlogs> _likedBlogsGetAllCustomeRepo;
        private readonly IGetAllWithConditionRepository<SharedBlogs> _sharedBlogsGetAllCustomeRepo;
        private readonly IGetAllWithConditionRepository<CommentedBlogs> _commentedBlogsGetAllCustomeRepo; 
        private readonly IGetAllWithConditionRepository<LikedBlogs> _likedBlogsGetAllWithConditionCustomeRepo;
        private readonly IGetAllWithConditionRepository<SharedBlogs> _sharedBlogsGetAllWithConditionCustomeRepo;
        private readonly IGetAllWithConditionRepository<CommentedBlogs> _commentedBlogsGetAllWithConditionCustomeRepo;
        private readonly IBlogsService<Blog> _blogService;
        public BlogsController(
            ICommonRepository<LikedBlogs> likedBlogsCommonRepo, 
            ICommonRepository<SharedBlogs> sharedBlogsCommonRepo,  
            ICommonRepository<CommentedBlogs> commentedBlogsCommonRepo,
            IGetAllWithConditionRepository<LikedBlogs> likedBlogsGetAllCustomeRepo,
            IGetAllWithConditionRepository<SharedBlogs> sharedBlogsGetAllCustomeRepo,
            IGetAllWithConditionRepository<CommentedBlogs> commentedBlogsGetAllCustomeRepo,
            IBlogsService<Blog> blogsService, 
            IGetAllWithConditionRepository<LikedBlogs> likedBlogsGetAllWithConditionCustomeRepo,
            IGetAllWithConditionRepository<SharedBlogs> sharedBlogsGetAllWithConditionCustomeRepo,
            IGetAllWithConditionRepository<CommentedBlogs> commentedBlogsGetAllWithConditionCustomeRepo
            )
        {
            _likedBlogsCommonRepo = likedBlogsCommonRepo;
            _sharedBlogsCommonRepo = sharedBlogsCommonRepo;
            _commentedBlogsCommonRepo = commentedBlogsCommonRepo;
            _likedBlogsGetAllCustomeRepo = likedBlogsGetAllCustomeRepo;
            _sharedBlogsGetAllCustomeRepo = sharedBlogsGetAllCustomeRepo;
            _commentedBlogsGetAllCustomeRepo = commentedBlogsGetAllCustomeRepo;
            _blogService = blogsService;
            _likedBlogsGetAllWithConditionCustomeRepo = likedBlogsGetAllWithConditionCustomeRepo;
            _sharedBlogsGetAllWithConditionCustomeRepo = sharedBlogsGetAllWithConditionCustomeRepo;
            _commentedBlogsGetAllWithConditionCustomeRepo = commentedBlogsGetAllWithConditionCustomeRepo;
        }

        [HttpGet("getallblogs")]
        public async Task<IActionResult> GetAllBlogs()
        {
            var result = await _blogService.GetAllBlogsAsync(); 

            return !result.Any() ? BadRequest("No blog was found") : Ok(result.Select(d => d.ToBlogGetDto()).OrderBy(x => x.CreatedAt));
        }

        [HttpGet("getfeaturedblogs")]
        public async Task<IActionResult> GetAllFeaturesBlogs()
        {
            var result = await _blogService.GetAllFeaturesBlogsAsync(); 

            return !result.Any() ? BadRequest("No blog was found") : Ok(result.Select(d => d.ToBlogGetDto()).OrderBy(x => x.CreatedAt));
        }

        [HttpGet("getblogspage")]
        public async Task<IActionResult> GetBlogsPage([FromQuery] int skip = 0, [FromQuery] int take = 10)
        { 
            if (skip < 0) return BadRequest("You can't skip with a negative value");

            var result = await _blogService.GetBlogsPageAsync(skip, take);
              
            return !result.Any() ? BadRequest("No blog was found") : Ok(result.Select(d => d.ToBlogGetDto()).OrderBy(x => x.CreatedAt));
        }

        [HttpGet("getblogbyid/{Id:int}")] 
        public async Task<IActionResult> GetBlogById([FromRoute] int Id)
        {
            var result = await _blogService.GetBlogByIdAsync(Id);
             
            return result is null ? BadRequest("No blog was found"): Ok(result.ToBlogGetDto()); 
        }

        [HttpGet("getuserblogs/{userId}")] 
        public async Task<IActionResult> GetUserBlogs([FromRoute] string userId)
        {
            var result = await _blogService.GetUserBlogsAsync(userId);

            return !result.Any() ? BadRequest($"No blog was found using this user id {userId}") : Ok(result.Select(d => d.ToBlogGetDto()).OrderBy(x => x.CreatedAt));
        } 

        [HttpGet("getallcategories")] 
        public async Task<IActionResult> GetAllCategory()
        {
            var result = await _blogService.GetAllCategoryAsync(); 

            return !result.Any() ? BadRequest("No category was found") : Ok(result.Select(x => x.GetCategoryDto()).OrderBy(x => x.Name));
        }

        [HttpPost("createblog")]
        public async Task<IActionResult> CreateBlog( [FromForm] CreateBlogDto dto)
        {
            var result = await _blogService.CreateBlogAsync(dto); 

            return result.SuccessOrNot ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpPut("updateblog/{userId}/{Id:int}")] 
        public async Task<IActionResult> UpdateBlog([FromRoute] string userId, [FromRoute] int Id, [FromForm] UpdateBlogDto dto)
        {
            var result = await _blogService.UpdateBlogAsync(userId, Id, dto);

            return result.SuccessOrNot ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpPost("addlike")] 
        public async Task<IActionResult> AddLike([FromQuery] int blogId, [FromQuery] string userId)
        {
            var result = await _blogService.AddLikeAsync(blogId, userId);

            return result.SuccessOrNot ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpPost("addshare")] 
        public async Task<IActionResult> AddShare([FromQuery] int blogId, [FromQuery] string userId)
        {
            var result = await _blogService.AddShareAsync(blogId, userId);

            return result.SuccessOrNot ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpPost("addcomment")]
        public async Task<IActionResult> AddComment([FromQuery] int blogId, [FromQuery] string userId, [FromBody] string comment)
        {
            var result = await _blogService.AddCommentAsync(blogId, userId, comment);

            return result.SuccessOrNot ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpDelete("removelike")]
        public async Task<IActionResult> RemoveLike([FromQuery] int blogId, [FromQuery] string userId)
        {
            var result = await _blogService.RemoveBlogPropertyAsync(blogId, userId, _likedBlogsGetAllCustomeRepo ,_likedBlogsCommonRepo);

            return result.SuccessOrNot ? Ok(result.Message) : BadRequest(result.Message); 
        }

        [HttpDelete("removeshare")] 
        public async Task<IActionResult> RemoveShare([FromQuery] int blogId, [FromQuery] string userId)
        {
            var result = await _blogService.RemoveBlogPropertyAsync(blogId, userId, _sharedBlogsGetAllCustomeRepo , _sharedBlogsCommonRepo);

            return result.SuccessOrNot ? Ok(result.Message) : BadRequest(result.Message);

        }

        [HttpDelete("removecomment")]
        public async Task<IActionResult> RemoveComment([FromQuery] int blogId, [FromQuery] string userId)
        {
            var result = await _blogService.RemoveBlogPropertyAsync(blogId, userId, _commentedBlogsGetAllCustomeRepo, _commentedBlogsCommonRepo);

            return result.SuccessOrNot ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpGet("islikedbyuser")]
        public async Task<IActionResult> IsLikedByUser([FromQuery] int blogId, [FromQuery] string userId)
        {
            var result = await _blogService.IsRelatedToUserAsync(blogId, userId, _likedBlogsGetAllWithConditionCustomeRepo);

            return result.SuccessOrNot ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpGet("issharedbyuser")]
        public async Task<IActionResult> IsSharedByUser([FromQuery] int blogId, [FromQuery] string userId)
        {
            var result = await _blogService.IsRelatedToUserAsync(blogId, userId, _sharedBlogsGetAllWithConditionCustomeRepo);

            return result.SuccessOrNot ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpGet("iscommentedbyuser")]
        public async Task<IActionResult> IsCommentedByUser([FromQuery] int blogId, [FromQuery] string userId)
        {
            var result = await _blogService.IsRelatedToUserAsync(blogId, userId, _commentedBlogsGetAllWithConditionCustomeRepo);

            return result.SuccessOrNot ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpDelete("deleteblog/{Id:int}")] 
        public async Task<IActionResult> DeleteBlog([FromRoute] int Id)
        {
            var result = await _blogService.DeleteBlogAsync(Id);

            return result.SuccessOrNot ? Ok(result.Message) : BadRequest(result.Message);
        }
    }
}
