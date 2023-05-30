using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PROJECT_ES.Service;
using PROJECT_ES.ViewModels;
public class MoviesController : Controller
{
    private readonly MovieRepository _movieRepository;
    

    public MoviesController(MovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
        
    }
    
    public async Task<IActionResult> SingleMovie(int movieId)
    {
        var movie = await _movieRepository.GetMovieAsync(movieId);
        // Logic to retrieve the movie details using the movieId
        // Pass the movie details to the view
        return View(movie);
    }
}