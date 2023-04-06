using System.Text.Json;
using TermoSolver.Extensions;
using TermoSolver.Models;
using TermoSolver.Services.Solver.Enums;
using TermoSolver.Services.Solver.Models;

namespace TermoSolver.Services.Solver.Implementations
{
    public class SolverService : ISolverService
    {
        private readonly ISolverFactory _solverFactory;

        public SolverService(ISolverFactory solverFactory)
        {
            _solverFactory = solverFactory;
        }

        public SolverExecutionResult ExecuteStrategy(SolverStrategy strategy, WordScore[] wordScores, string initialWord, string finalWord)
        {
            var solver = _solverFactory.CreateForSolverStrategyType(strategy);
            var state = new SolverExecutionState(wordScores, initialWord);

            while (state.Status == SolverExecutionStatus.Running)
            {
                CheckWord(finalWord, state);
                GuessWord(solver, state);
            }

            return new SolverExecutionResult(
                InitialWord: initialWord,
                FinalWord: finalWord,
                Attempts: state.Attempts,
                IsSuccessful: state.Status == SolverExecutionStatus.Successful);
        }

        private void GuessWord(IWordSolver solver, SolverExecutionState state)
        {
            state.Attempts.Add((WordState[]) state.Candidate.Clone());
            if (state.Candidate.Count(s => s.State == CharacterState.RightPosition) == state.Candidate.Length)
            {
                state.Status = SolverExecutionStatus.Successful;
                return;
            }

            solver.IterateFilter(state.GlobalFilter, state.Candidate);
            var newCandidate = solver.GetNextWord(state.WordScores, state.GlobalFilter);

            if (newCandidate == null)
            {
                state.Status = SolverExecutionStatus.Failed;
                return;
            }

            for (int i = 0; i < newCandidate?.Length; i++)
            {
                state.Candidate[i] = new WordState()
                { 
                    Character = newCandidate[i], 
                    State = state.Candidate[i].State == CharacterState.RightPosition ? CharacterState.RightPosition : CharacterState.WrongCharacter, 
                };
            }
        }

        private void CheckWord(string finalWord, SolverExecutionState state)
        {
            // first sweep to get the right placed and wrong characters in the candidate
            for (int i = 0; i < state.Candidate.Length; i++)
            {
                char candidate = state.Candidate[i].Character;
                var wordState = CharacterState.WrongCharacter;
                if (!finalWord.Contains(candidate))
                {
                    wordState = CharacterState.WrongCharacter;
                }
                else if (candidate == finalWord[i])
                {
                    wordState = CharacterState.RightPosition;
                }
                else
                {
                    wordState = CharacterState.Unknown;
                }

                state.Candidate[i].State = wordState;
            }

            for (int i = 0; i < state.Candidate.Length; i++)
            {
                if (state.Candidate[i].State == CharacterState.Unknown)
                {
                    var candidate = state.Candidate[i].Character;
                    var nCharsInFinalWord = finalWord.Count(x => x == candidate);
                    var newState = CharacterState.Unknown;
                    var nCharMissingFromWordCandidate = nCharsInFinalWord - state.Candidate.Count(x => x.Character == candidate && (x.State == CharacterState.RightPosition || x.State == CharacterState.WrongPosition));
                    newState = nCharMissingFromWordCandidate > 0 ? CharacterState.WrongPosition : CharacterState.WrongCharacter;
                    state.Candidate[i].State = newState;
                }
            }
        }
    }
}
