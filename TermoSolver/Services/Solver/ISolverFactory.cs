using TermoSolver.Services.Solver.Enums;

namespace TermoSolver.Services.Solver
{
    public interface ISolverFactory
    {
        public IWordSolver CreateForSolverStrategyType(SolverStrategy strategy);
    }
}
