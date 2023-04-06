using TermoSolver.Models;
using TermoSolver.Services.Solver.Enums;
using TermoSolver.Services.Solver.Models;

namespace TermoSolver.Services.Solver
{
    public interface ISolverService
    {
        public SolverExecutionResult ExecuteStrategy(SolverStrategy strategy,  WordScore[] wordScores, string initialWord, string finalWord);
    }
}
