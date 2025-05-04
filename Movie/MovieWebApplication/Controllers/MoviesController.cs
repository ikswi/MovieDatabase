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
public class MoviesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public MoviesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // а. Список фильмов по алфавиту
    [HttpGet("alphabetical")]
    public async Task<ActionResult<IEnumerable<Movie>>> GetAlphabetical()
    {
        return await _context.Movies
            .OrderBy(m => m.Title)
            .ToListAsync();
    }

    // б. Список фильмов по дате выхода
    [HttpGet("by-date")]
    public async Task<ActionResult<IEnumerable<Movie>>> GetByReleaseDate()
    {
        return await _context.Movies
            .OrderBy(m => m.ReleaseDate)
            .ToListAsync();
    }

    // в. Популярные фильмы (по голосам)
    [HttpGet("popular")]
    public async Task<ActionResult<IEnumerable<Movie>>> GetPopular()
    {
        return await _context.Movies
            .OrderByDescending(m => m.Votes.Count)
            .Take(10)
            .ToListAsync();
    }

    // е. Информация о фильме
    [HttpGet("{id}")]
    public async Task<ActionResult<Movie>> GetMovie(int id)
    {
        var movie = await _context.Movies
            .Include(m => m.MovieActors)
            .ThenInclude(ma => ma.Actor)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (movie == null)
            return NotFound();
        return Ok(movie);
    }

    // Добавление актера в фильм
    [HttpPost("{movieId}/actors/{actorId}")]
    public async Task<IActionResult> AddActor(int movieId, int actorId)
    {
        _context.MovieActors.Add(new MovieActor
        {
            MovieId = movieId,
            ActorId = actorId
        });
        await _context.SaveChangesAsync();
        return NoContent();
    }
}