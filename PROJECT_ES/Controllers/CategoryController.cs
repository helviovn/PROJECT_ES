using Microsoft.AspNetCore.Mvc;
using PROJECT_ES.Service;

public class CategoryController : Controller
{
    private readonly CompetitionDetailsRepository _competitionDetailsRepository;

    public CategoryController(CompetitionDetailsRepository competitionDetailsRepository)
    {
        _competitionDetailsRepository = competitionDetailsRepository;
    }

    public async Task<IActionResult> CategoriesPage(int competitionId)
    {
        var categories = await _competitionDetailsRepository.GetAllCategoriesAsync(competitionId);
        var movies = await _competitionDetailsRepository.GetAllMoviesAsync(competitionId);

        return View("CategoriesPage", categories);
    }
}