using Microsoft.AspNetCore.Mvc;
using PROJECT_ES.Service;

namespace PROJECT_ES.Controllers;

public abstract class BaseController : Controller
{
    protected CompetitionRepository _competitionRepository;
    protected CompetitionDetailsRepository _competitionDetailsRepository;
    protected CategoryRepository _categoryRepository;

    public BaseController(CompetitionRepository competitionRepository, 
        CompetitionDetailsRepository competitionDetailsRepository,
        CategoryRepository categoryRepository)
    {
        _competitionRepository = competitionRepository;
        _competitionDetailsRepository = competitionDetailsRepository;
        _categoryRepository = categoryRepository;
    }

    protected abstract Task<IActionResult> GetViewModel(int competitionId, int categoryId);

    public async Task<IActionResult> BaseAction(int competitionId, int categoryId)
    {
        var viewModel = await GetViewModel(competitionId, categoryId);

        if (viewModel == null)
        {
            return NotFound();
        }

        return View("FirstPage");
    }
}
