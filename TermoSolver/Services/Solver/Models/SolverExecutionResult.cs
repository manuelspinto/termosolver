namespace TermoSolver.Services.Solver.Models
{
    public record SolverExecutionResult(
        string InitialWord, 
        string FinalWord,
        List<WordState[]> Attempts,
        bool IsSuccessful = false
    );
}
