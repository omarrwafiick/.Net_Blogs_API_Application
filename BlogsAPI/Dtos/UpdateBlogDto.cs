using System.ComponentModel.DataAnnotations;

namespace BlogsAPI.Dtos
{
    public class UpdateBlogDto
    { 

        private string _title;
        private string _description;
        private string _content;
        private IFormFile _image;
        private int _categoryId;

        [MaxLength(30)]
        [MinLength(3)]
        public string Title
        {
            get => _title;
            set => _title = SantizeData.SanitizeString(value);
        }

        [MaxLength(150)]
        [MinLength(10)]
        public string Description
        {
            get => _description;
            set => _description = SantizeData.SanitizeString(value);
        }

        [MaxLength(400)]
        [MinLength(10)]
        public string Content
        {
            get => _content;
            set => _content = SantizeData.SanitizeString(value);
        }
         
        public IFormFile? Image
        {
            get => _image;
            set => _image = value;
        }
          
        public int CategoryId
        {
            get => _categoryId;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(CategoryId), "CategoryId must be greater than zero.");
                }
                _categoryId = value;
            }
        }
    }
}
