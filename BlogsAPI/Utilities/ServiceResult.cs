namespace BlogsAPI.Utilities
{
    public class ServiceResult
    {
        public bool SuccessOrNot { get; private set; }
        public string Message { get; private set; }
        public object Data { get; private set; }
         
        public static ServiceResult Success(string message = null, object data = null)
        {
            return new ServiceResult
            {
                SuccessOrNot = true,
                Message = message,
                Data = data
            };
        }
         
        public static ServiceResult Failure(string message, object data = null)
        {
            return new ServiceResult
            {
                SuccessOrNot = false,
                Message = message,
                Data = data
            };
        }
    }
}
