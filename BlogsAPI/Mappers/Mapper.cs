 

namespace BlogsAPI.Mappers
{
    public static class Mapper
    {
        public static Blog FromBlogCreateDto(this CreateBlogDto blog)
        {
            using var datastream = new MemoryStream();
            blog.Image.CopyToAsync(datastream).GetAwaiter().GetResult();
            return new Blog
            {
                Title = blog.Title,
                Description = blog.Description,
                Content = blog.Content,
                IsFeatured = blog.IsFeatured,
                Image = datastream.ToArray(),
                CategoryId = blog.CategoryId,
                AppUserId = blog.UserId,
            };
        }

        public static GetBlogDto ToBlogGetDto(this Blog blog)
        {
            GetBlogDto obj = new()
            {
                Id = blog.Id,
                Title = blog.Title,
                Description = blog.Description,
                Content = blog.Content, 
                CategoryId = blog.CategoryId,
                IsFeatured = blog.IsFeatured,
                Gender = blog.AppUser.Gender ? "male" : "female",
                LikedBy = blog.LikedBlogsTbl.Select(x=>x.AppUserId).ToList(),
                SharedBy = blog.SharedBlogsTbl.Select(x => x.AppUserId).ToList(),
                CommentsInfo = blog.CommentedBlogsTbl.Select(x => new GetCommentedDto { AppUserId = x.AppUserId, Comment = x.Comment}).ToList(),
                CreatedAt = blog.CreatedAt
            };
            obj.Image = Convert.ToBase64String(blog.Image);
            return obj;
        }

        public static AppUser FromAppUserDto(this RegisterDto data)
        {
            return new AppUser
            {
                FullName = data.FullName,
                UserName = data.UserName,
                Email = data.Email,
                PhoneNumber = data.Phone,
                Gender = data.Gender 
            };
        } 

        public static GetUsersDto GetAllUsersDto(this AppUser user, List<string> roles)
        {
            return new GetUsersDto
            {
                Userid = user.Id,
                Username=user.UserName,
                Fullname = user.FullName,
                Email = user.Email,
                Roles = roles
            };
        } 

        public static AppData AppDataDto(this AppData data,string Id ,string Email ,string Token ,string UserName,string FullName,bool Gender,bool notified)
        {
            return new AppData
            {
               Id = Id,
               Email = Email,   
               Token = Token,
               UserName=UserName,
               FullName = FullName,
               Gender = Gender ? "male" : "female",
               Notified = notified
            };
        }

        public static GetCategoryDto GetCategoryDto(this Category model)
        {
            return new GetCategoryDto
            {
              Id=model.Id,
              Name=model.Name
            };
        }

        public static Blog FromUpdateBlogToBlog(this UpdateBlogDto dto, Blog blog)
        { 
            blog.CategoryId = dto.CategoryId;
            blog.Description = dto.Description;
            blog.Title = dto.Title; 
            if (dto.Image is not null)
            {
                using var datastream = new MemoryStream();
                dto.Image.CopyToAsync(datastream).GetAwaiter().GetResult();
                blog.Image = datastream.ToArray();
            }
            return blog;
        }
    }
}
