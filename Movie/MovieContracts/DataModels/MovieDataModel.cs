using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieContracts.DataModels;

public class MovieDataModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime ReleaseDate { get; set; }
    public double Rating { get; set; }
    public List<ActorDataModel> Actors { get; set; }
}
