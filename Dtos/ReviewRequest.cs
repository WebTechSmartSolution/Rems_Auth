namespace Rems_Auth.Dtos
{
    public class ReviewRequest
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string name { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
    }

}
