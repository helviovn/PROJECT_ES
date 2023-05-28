using System.Collections;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using PROJECT_ES.Data;
using PROJECT_ES.Service;

public class CompetitionController  : Controller
{

    private readonly CompetitionRepository _competitionRepository;
    private readonly string _connectionString;

    public CompetitionController(IConfiguration configuration, CompetitionRepository competitionRepository)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
        _competitionRepository = competitionRepository;
    }
    
}