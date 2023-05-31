using PROJECT_ES.Service;

namespace PROJECT_ES.Interfaces
{
    public interface ICompetitionRepositoryFactory
    {
        CompetitionRepository Create(IConfiguration connectionString);
    }
}