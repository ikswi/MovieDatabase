using MovieWebApplication.Enums;

namespace MovieWebApplication.Models;

public class Vote
{
    public int Id { get; set; }
    public int UserId { get; set; }

    public int MovieId { get; set; }  
    public Movie Movie { get; set; }

    public User User { get; set; }
    public int ContentId { get; set; }
    public ContentType ContentType { get; set; }
    public DateTime VoteDate { get; set; } = DateTime.UtcNow;
}
