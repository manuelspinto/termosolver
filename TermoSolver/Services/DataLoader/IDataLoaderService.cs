using TermoSolver.Models;

namespace TermoSolver.Services.DataLoader
{
    public interface IDataLoaderService
    {
        public Task<WordScore[]> GetAllWordScoreAsync();
    }
}
