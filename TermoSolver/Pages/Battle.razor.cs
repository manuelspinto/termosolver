using Microsoft.AspNetCore.Components;
using TermoSolver.Models;
using TermoSolver.Services.Solver;
using TermoSolver.Services.Solver.Enums;
using TermoSolver.Services.Solver.Models;
using System.Net.Http.Json;

namespace TermoSolver.Pages
{
    public partial class Battle 
    {
        private IEnumerable<WordScore>? WordScores;
        private List<SolverExecutionResult> ExecutionResults = new List<SolverExecutionResult>();
        private const int nWords = 2;
        private int wordCount = 0;

        [Inject]
        protected ISolverService SolverService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            WordScores = await Http.GetFromJsonAsync<WordScore[]>("sample-data/word-score.json");
            WordScores = WordScores.Distinct(new WordScoreComparer());
            wordCount = WordScores.Count();

            var random = new Random();

            for (int i = 0;i < nWords; i++)
            {
                var startWord = WordScores.ElementAt(random.Next(wordCount)).Word;
                var finalWord = WordScores.ElementAt(random.Next(wordCount)).Word;

                ExecutionResults.Add(
                            SolverService.ExecuteStrategy(SolverStrategy.Pepperoni, WordScores.ToArray(), startWord, finalWord));
            }
        }
    }
}


