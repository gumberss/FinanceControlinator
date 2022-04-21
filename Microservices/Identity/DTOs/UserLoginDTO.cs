namespace Identity.DTOs
{
    public record UserLoginDTO
    {
        public String UserName { get; set; }

        public String Password { get; set; }

        public String Email { get; set; }
    }
}
