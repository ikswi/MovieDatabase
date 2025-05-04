using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieWebApplication.Data;
using MovieWebApplication.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieWebApplication.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActorsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ActorsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // г. Популярные актеры
    [HttpGet("popular")]
    public async Task<ActionResult<IEnumerable<Actor>>> GetPopular()
    {
        return await _context.Actors
            .OrderByDescending(a => a.Votes.Count)
            .Take(10)
            .ToListAsync();
    }

    // д. Фильмы актера
    [HttpGet("{id}/movies")]
    public async Task<ActionResult<IEnumerable<Movie>>> GetActorMovies(int id)
    {
        var actor = await _context.Actors
            .Include(a => a.MovieActors)
            .ThenInclude(ma => ma.Movie)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (actor == null)
            return NotFound();
        return actor.MovieActors.Select(ma => ma.Movie).ToList();
    }

    // е. Информация об актере
    [HttpGet("{id}")]
    public async Task<ActionResult<Actor>> GetActor(int id)
    {
        var actor = await _context.Actors.FindAsync(id);
        if (actor == null)
            return NotFound();
        return Ok(actor);
    }
}