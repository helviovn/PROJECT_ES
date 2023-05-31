using PROJECT_ES.Service;
using PROJECT_ES.Data;
using PROJECT_ES.Interfaces;

namespace PROJECT_ES.Factories
{
    public class RepositoryFactory : IRepositoryFactory
    {
        public CompetitionRepository Create(IConfiguration connectionString)
        {
            return new CompetitionRepository(connectionString);
        }
    }
}