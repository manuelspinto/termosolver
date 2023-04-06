using TermoSolver.Services.Solver.Enums;
using TermoSolver.Services.Solver.SolverStrategies;

namespace TermoSolver.Services.Solver.Implementations
{
    public class SolverFactory : ISolverFactory
    {
        public IWordSolver CreateForSolverStrategyType(SolverStrategy strategy)
        {
            switch (strategy) 
            {
                case SolverStrategy.Pepperoni:
                    return new PepperoniSolver();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
