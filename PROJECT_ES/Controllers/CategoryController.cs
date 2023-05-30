using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PROJECT_ES.Service;
using PROJECT_ES.ViewModels;
using PROJECT_ES.Interfaces;

public class CategoryController : Controller
{
    private readonly ICompetitionRepositoryFactory _competitionRepositoryFactory;
    private readonly CompetitionDetailsRepository _competitionDetailsRepository;
    private readonly IConfiguration _connectionString;

    public CategoryController(ICompetitionRepositoryFactory competitionRepositoryFactory, CompetitionDetailsRepository competitionDetailsRepository, IConfiguration configuration)
    {
        _competitionRepositoryFactory = competitionRepositoryFactory;
        _competitionDetailsRepository = competitionDetailsRepository;
        _connectionString = configuration;
    }

    public async Task<IActionResult> CategoriesPage(int competitionId)
    {
        var competitionRepository = _competitionRepositoryFactory.Create(_connectionString);
        
        var competition = await competitionRepository.GetCompetitionByIdAsync(competitionId);
        var categories = await _competitionDetailsRepository.GetAllCategoriesAsync(competitionId);
        var movies = await _competitionDetailsRepository.GetAllMoviesAsync(competitionId);

        var viewModel = new CategoryViewModel
        {
            Competition = competition,
            Categories = categories,
            Movies = movies
        };

        return View("CategoriesPage", viewModel);
    }
}