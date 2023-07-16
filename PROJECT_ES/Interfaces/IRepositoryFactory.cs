using PROJECT_ES.Service;

namespace PROJECT_ES.Interfaces
{
    public interface IRepositoryFactory
    {
        CompetitionRepository Create(IConfiguration connectionString);
    }
}