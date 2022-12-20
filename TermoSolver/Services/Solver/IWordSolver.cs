using TermoSolver.Models;

namespace TermoSolver.Services.Solver
{
    public interface IWordSolver
    {
        public string? GetNextWord(IEnumerable<WordScore> words, WordFilter filter);

        public IEnumerable<WordScore>? GetNextWords(IEnumerable<WordScore> words, WordFilter filter);

        public void IterateFilter(WordFilter filter, WordState[] wordState);
    }
}
