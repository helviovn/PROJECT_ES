using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PROJECT_ES.Service;
using PROJECT_ES.ViewModels;
using PROJECT_ES.Interfaces;
using Microsoft.AspNetCore.Identity;

public class CategoryController : Controller
{
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly CompetitionDetailsRepository _competitionDetailsRepository;
    private readonly IConfiguration _connectionString;
    private readonly UserManager<IdentityUser> _userManager;

    public CategoryController(IRepositoryFactory competitionRepositoryFactory, CompetitionDetailsRepository competitionDetailsRepository, IConfiguration configuration, UserManager<IdentityUser> userManager)
    {
        _repositoryFactory = competitionRepositoryFactory;
        _competitionDetailsRepository = competitionDetailsRepository;
        _connectionString = configuration;
        _userManager = userManager;
    }

    public async Task<IActionResult> CategoriesPage(int competitionId)
    {
        var competitionRepository = _repositoryFactory.Create(_connectionString);
        
        var competition = await competitionRepository.GetCompetitionByIdAsync(competitionId);
        var categories = await _competitionDetailsRepository.GetAllCategoriesAsync(competitionId);
        var movies = await _competitionDetailsRepository.GetAllMoviesAsync(competitionId);
        
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
        
        var images = new List<string>
        {
            "/images/categories/11.jpg",
            "/images/categories/22.jpg",
            "/images/categories/33.png",
            "/images/categories/44.png",
            "/images/categories/55.png",
            "/images/categories/66.png",
        };


        // Assign images to categories
        for (int i = 0; i < categories.Count(); i++)
        {
            var category = categories.ElementAt(i);
            category.Image = images.ElementAtOrDefault(i % images.Count);
        }


        var viewModel = new CategoryViewModel
        {
            Competition = competition,
            Categories = categories,
            Movies = movies
        };

        return View("CategoriesPage", viewModel);
    }
}