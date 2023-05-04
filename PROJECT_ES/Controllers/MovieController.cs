using Microsoft.AspNetCore.Mvc;
using PROJECT_ES.Data;
using PROJECT_ES.Service;

namespace PROJECT_ES.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MovieController : ControllerBase
{
    private readonly MovieRepository _repository;
    private readonly MovieService _service;

    public MovieController(MovieRepository repository, MovieService service)
    {
        _repository = repository;
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Movie>> GetMovieAsync(int id)
    {
        var movie = await _repository.GetMovieAsync(id);

        if (movie == null)
        {
            return NotFound();
        }

        return Ok(movie);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Movie>>> GetMoviesAsync()
    {
        var movies = await _repository.GetMoviesAsync();

        if (!movies.Any())
        {
            return NoContent();
        }

        return Ok(movies);
    }

    [HttpPost]
    public async Task<ActionResult<Movie>> AddMovieAsync(Movie movie)
    {
        var id = await _repository.AddMovieAsync(movie);
        movie.Id = id;

        return CreatedAtAction(nameof(GetMovieAsync), new { id }, movie);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMovieAsync(int id, Movie movie)
    {
        if (id != movie.Id)
        {
            return BadRequest();
        }

        await _repository.UpdateMovieAsync(movie);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovieAsync(int id)
    {
        await _repository.DeleteMovieAsync(id);

        return NoContent();
    }

    [HttpPost("search")]
    public async Task<ActionResult<Movie>> SearchMovieAsync([FromBody] string title)
    {
        var movie = await _service.GetMovieAsync(title);

        if (movie == null)
        {
            return NotFound();
        }

        return Ok(movie);
    }
}