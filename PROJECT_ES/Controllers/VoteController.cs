using System.Collections;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PROJECT_ES.Data;
using PROJECT_ES.Service;
using PROJECT_ES.ViewModels;
using Microsoft.AspNetCore.Mvc;


public class VoteController  : Controller
{
    
    private readonly CompetitionRepository _competitionRepository;
    private readonly CompetitionDetailsRepository _competitionDetailsRepository;
    private readonly CategoryRepository _categoryRepository;

    public VoteController(CompetitionRepository competitionRepository, CompetitionDetailsRepository competitionDetailsRepository, CategoryRepository categoryRepository)
    {
        _competitionRepository = competitionRepository;
        _competitionDetailsRepository = competitionDetailsRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<IActionResult> VotingPage(int competitionId, int categoryId)
    {
        var competition = await _competitionRepository.GetCompetitionByIdAsync(competitionId);
        var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
        var movies = await _competitionDetailsRepository.GetAllMoviesAsync(competitionId);

        var viewModel = new VoteViewModel()
        {
            Competition = competition,
            Category = category,
            Movies = movies
        };

        return View("VotingPage", viewModel);
    }
   
    
}