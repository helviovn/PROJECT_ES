
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using PROJECT_ES.Service;
using PROJECT_ES.ViewModels;
using Microsoft.AspNetCore.Identity;


public class CompetitionsController : Controller
{
    private readonly string _connectionString;
    private readonly CompetitionRepository _competitionRepository;
    private readonly CompetitionDetailsRepository _competitionDetailsRepository;
    private readonly CategoryRepository _categoryRepository;
    
    private readonly UserManager<IdentityUser> _userManager;

    public CompetitionsController(IConfiguration configuration, CompetitionRepository competitionRepository,
        CompetitionDetailsRepository competitionDetailsRepository, CategoryRepository categoryRepository,UserManager<IdentityUser> userManager)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");

        _competitionRepository = competitionRepository;
        _competitionDetailsRepository = competitionDetailsRepository;
        _categoryRepository = categoryRepository;
        _userManager = userManager;
    }

    public async Task<IActionResult> CompetitionClosed(int competitionId)
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
        
        var competition = await _competitionRepository.GetCompetitionByIdAsync(competitionId);
        var category = await _competitionDetailsRepository.GetAllCategoriesAsync(competitionId);
        var movies = await _competitionDetailsRepository.GetAllMoviesAsync(competitionId);
        var statistics = await _competitionDetailsRepository.StatesByCompId(competitionId);
        
        var viewModel = new CompetitionsViewModel()
        {
            Competition = competition,
            Category = category,
            Movies = movies,
            statistics = statistics
            
        };
        return View("CompetitionClosed", viewModel);
    }

}
    


 

   
    
