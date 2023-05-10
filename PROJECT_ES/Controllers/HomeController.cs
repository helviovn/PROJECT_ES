using System.Collections;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PROJECT_ES.Data;
using PROJECT_ES.Service;

public class HomeController  : Controller
{
    
    private readonly CompetitionRepository _competitionRepository;

    public HomeController(CompetitionRepository competitionRepository)
    {
        _competitionRepository = competitionRepository;
        
    }

    public async Task<IActionResult> FirstPage()
    {
        var competitions = await _competitionRepository.GetCompetitionsAsync();
        return View(competitions);
    }
    
    public async Task<IActionResult> Home()
    {
        return RedirectToAction("FirstPage");
    }
    
    
}