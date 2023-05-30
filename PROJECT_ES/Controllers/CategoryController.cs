using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PROJECT_ES.Service;
using PROJECT_ES.ViewModels;
public class CategoryController : Controller
{
    private readonly CompetitionRepository _competitionRepository;
    private readonly CompetitionDetailsRepository _competitionDetailsRepository;

    public CategoryController(CompetitionRepository competitionRepository, CompetitionDetailsRepository competitionDetailsRepository)
    {
        _competitionRepository = competitionRepository;
        _competitionDetailsRepository = competitionDetailsRepository;
    }

    public async Task<IActionResult> CategoriesPage(int competitionId)
    {
        var competition = await _competitionRepository.GetCompetitionByIdAsync(competitionId);
        var categories = await _competitionDetailsRepository.GetAllCategoriesAsync(competitionId);
        var movies = await _competitionDetailsRepository.GetAllMoviesAsync(competitionId);
        
        
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
            Movies = movies,
        };

        return View("CategoriesPage", viewModel);

    }
}