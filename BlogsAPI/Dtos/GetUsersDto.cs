namespace BlogsAPI.Dtos
{
    public class GetUsersDto
    {
        public string Userid { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public List<string> Roles { get; set; }
    }
}
