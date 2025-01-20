
namespace PhotoCommunity2025.Models
{
    public class User
    {
        public int UserId { get; set; }

        public required string LastName { get; set; }

        public required string FirstName { get; set; }

        public required string MiddleName { get; set; }

        public required string Login { get; set; }

        public required string Password { get; set; }

        public required string Cameras { get; set; }

        public required string Lenses { get; set; }

        public virtual List<Photo> Photos { get; set; } = new List<Photo>();
    }
}
