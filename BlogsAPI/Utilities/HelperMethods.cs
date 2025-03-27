
namespace BlogsAPI.Services
{
    public static class HelperMethods
    {
        public static IFormFile ToFormFile(this byte[] imageBytes, string fileName = "image.jpg")
        {
            var stream = new MemoryStream(imageBytes);
            return new FormFile(stream, 0, imageBytes.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpg"
            };
        }

        public static bool ValidateImage(IFormFile Image) => AppConstants.ALLOWEDEXTENSIONS.Contains(Path.GetExtension(Image.FileName.ToLower())) && AppConstants.MAXALLOWEDIMAGESIZE >= Image.Length;
        
    }
}
