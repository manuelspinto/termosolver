using System.Text.Json;
using TermoSolver.Extensions;
using TermoSolver.Models;

namespace TermoSolver.Services.Solver.Models
{
    public class SolverExecutionState 
    {
        private const string WordScoreFile = "sample-data/word-score.json";

        public WordScore[] WordScores { get; set; }

        public WordFilter GlobalFilter { get; set; }

        public List<WordState[]> Attempts { get; set; }

        public WordState[] Candidate { get; set; }

        public SolverExecutionStatus Status { get; set; }

        public SolverExecutionState(WordScore[] wordScores, string initialWord)
        {
            WordScores= wordScores;

            GlobalFilter = new WordFilter();
            Attempts = new List<WordState[]>();

            Candidate = initialWord.ToInitialWordState();

            Status = SolverExecutionStatus.Running;
        }
    }
}
