using System.Collections.Generic;

namespace MovieWebApplication.Models;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string Description { get; set; }
    public int Duration { get; set; } // in minutes
    public string PosterUrl { get; set; }

    public ICollection<MovieActor> MovieActors { get; set; }
    public ICollection<Vote> Votes { get; set; }
}

