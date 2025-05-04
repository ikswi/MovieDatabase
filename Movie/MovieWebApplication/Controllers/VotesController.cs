using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieWebApplication.Data;
using MovieWebApplication.Models;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieWebApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VotesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VotesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("movies/{movieId}")]
        public async Task<IActionResult> VoteForMovie(int movieId, [FromHeader] string authorization)
        {
            var userId = ValidateToken(authorization);
            if (userId == null)
                return Unauthorized();

            if (await _context.Votes.AnyAsync(v => v.UserId == userId && v.MovieId == movieId))
                return BadRequest("Already voted");

            _context.Votes.Add(new Vote
            {
                UserId = userId.Value,
                MovieId = movieId,
                VoteDate = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return NoContent();
        }

        private int? ValidateToken(string token)
        {
            try
            {
                var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(token));
                var parts = decoded.Split(':');
                if (parts.Length != 3) return null;

                if (int.TryParse(parts[0], out var userId))
                    return userId;

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}