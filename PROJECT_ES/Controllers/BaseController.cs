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

        if (competitionId == 0 && categoryId == 0)
        {
            return View("FirstPage");
        }
        else
        {
            // Redirecionar para a página da categoria com base na competição selecionada
            return View("~/Views/Category/CategoryPage.cshtml");
        }
    }
}
