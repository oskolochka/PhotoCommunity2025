using PhotoCommunity2025.Models;


public class Photo
{
    public int PhotoId { get; set; } 
    public int UserId { get; set; } 
    public string Title { get; set; } 
    public string Description { get; set; } 
    public string Tags { get; set; } 
    public string FilePath { get; set; }

    public virtual User User { get; set; } 

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

}