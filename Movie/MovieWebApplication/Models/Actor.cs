namespace MovieWebApplication.Models;

public class Actor
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime BirthDate { get; set; }
    public string Bio { get; set; }

    public ICollection<MovieActor> MovieActors { get; set; }
    public ICollection<Vote> Votes { get; set; }
}
