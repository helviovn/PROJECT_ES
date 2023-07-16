using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PROJECT_ES.Service;
using PROJECT_ES.ViewModels;
using Microsoft.AspNetCore.Identity;
public class MoviesController : Controller
{
    private readonly MovieRepository _movieRepository;
    private readonly UserManager<IdentityUser> _userManager;

    public MoviesController(MovieRepository movieRepository,UserManager<IdentityUser> userManager)
    {
        _movieRepository = movieRepository;
        _userManager = userManager;
    }
    
    public async Task<IActionResult> SingleMovie(int movieId)
    {
        
        var user = await _userManager.GetUserAsync(User);
        if (user != null)
        {
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            ViewBag.IsAdmin = isAdmin;
        }
        else
        {
            // Handle the case where the user is not found or not authenticated
            ViewBag.IsAdmin = false; // Set a default value or handle accordingly
        }
        var movie = await _movieRepository.GetMovieAsync(movieId);
        // Logic to retrieve the movie details using the movieId
        // Pass the movie details to the view
        return View(movie);
    }
}