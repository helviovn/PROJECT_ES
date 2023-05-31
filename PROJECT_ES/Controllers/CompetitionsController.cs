
using Microsoft.AspNetCore.Mvc;

using PROJECT_ES.Service;
using PROJECT_ES.ViewModels;


public class CompetitionsController : Controller
{
    private readonly string _connectionString;
    private readonly CompetitionRepository _competitionRepository;
    private readonly CompetitionDetailsRepository _competitionDetailsRepository;
    private readonly CategoryRepository _categoryRepository;

    public CompetitionsController(IConfiguration configuration, CompetitionRepository competitionRepository,
        CompetitionDetailsRepository competitionDetailsRepository, CategoryRepository categoryRepository)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");

        _competitionRepository = competitionRepository;
        _competitionDetailsRepository = competitionDetailsRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<IActionResult> CompetitionClosed(int competitionId)
    {
        
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
    


 

   
    
