using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using TermoSolver.Extensions;
using TermoSolver.Models;
using TermoSolver.Services.Solver;

namespace TermoSolver.Pages
{
    public partial class Termo
    {
        private IEnumerable<WordScore>? WordScores;
        private WordFilter GlobalFilter = new WordFilter();
        private bool NoMoreWords = false;
        private bool IsSuccess = false;
        private WordState[]? WordCandidate;

        [Parameter]
        [SupplyParameterFromQuery(Name = "word")]
        public string StartWord { get; set; } = string.Empty;

        [Parameter]
        [SupplyParameterFromQuery(Name = "final")]

        public string FinalWord { get; set; } = string.Empty;
        protected override async Task OnInitializedAsync()
        {
            WordScores = await Http.GetFromJsonAsync<WordScore[]>("sample-data/word-score.json");
            WordScores = WordScores.Distinct(new WordScoreComparer());
            var startWord = (!string.IsNullOrEmpty(StartWord) && StartWord.Length == 5 && Regex.IsMatch(StartWord, @"^[a-zA-Z]+$") && WordScores.Any(x => x.Word.Equals(StartWord, StringComparison.OrdinalIgnoreCase))) ? StartWord.ToUpper() : WordSolver.GetNextWord(WordScores, GlobalFilter);
            WordCandidate = startWord.ToInitialWordState();
            if (!string.IsNullOrEmpty(FinalWord) && FinalWord.Length == 5 && Regex.IsMatch(FinalWord, @"^[a-zA-Z]+$"))
            {
                FinalWord = FinalWord.ToUpper();
                while (IsSuccess == false && NoMoreWords == false)
                {
                    CheckWord();
                    GuessWord();
                }
            }
        }

        private Dictionary<CharacterState, string> StateClass = new Dictionary<CharacterState, string>()
        {{CharacterState.RightPosition, "btn-success"}, {CharacterState.WrongPosition, "btn-warning"}, {CharacterState.WrongCharacter, "btn-dark"}};
        private List<WordState[]> Attempts = new List<WordState[]>();
        private void CheckWord()
        {
            // first sweep to get the right placed and wrong characters in the candidate
            for (int i = 0; i < WordCandidate.Length; i++)
            {
                char candidate = WordCandidate[i].Character;
                var state = CharacterState.WrongCharacter;
                if (!FinalWord.Contains(candidate))
                {
                    state = CharacterState.WrongCharacter;
                }
                else if (candidate == FinalWord[i])
                {
                    state = CharacterState.RightPosition;
                }
                else
                {
                    state = CharacterState.Unknown;
                }

                WordCandidate[i].State = state;
            }

            for (int i = 0; i < WordCandidate.Length; i++)
            {
                if (WordCandidate[i].State == CharacterState.Unknown)
                {
                    var candidate = WordCandidate[i].Character;
                    var nCharsInFinalWord = FinalWord.Count(x => x == candidate);
                    var newState = CharacterState.Unknown;
                    var nCharMissingFromWordCandidate = nCharsInFinalWord - WordCandidate.Count(x => x.Character == candidate && (x.State == CharacterState.RightPosition || x.State == CharacterState.WrongPosition));
                    newState = nCharMissingFromWordCandidate > 0 ? CharacterState.WrongPosition : CharacterState.WrongCharacter;
                    WordCandidate[i].State = newState;
                }
            }
        }

        private void GuessWord()
        {
            Attempts.Add((WordState[])WordCandidate.Clone());
            if (WordCandidate.Count(s => s.State == CharacterState.RightPosition) == WordCandidate.Length)
            {
                IsSuccess = true;
                return;
            }

            WordSolver.IterateFilter(GlobalFilter, WordCandidate);
            var candidate = WordSolver.GetNextWord(WordScores, GlobalFilter);
            if (candidate == null)
            {
                NoMoreWords = true;
                return;
            }

            for (int i = 0; i < candidate?.Length; i++)
            {
                WordCandidate[i] = new WordState()
                {Character = candidate[i], State = WordCandidate[i].State == CharacterState.RightPosition ? CharacterState.RightPosition : CharacterState.WrongCharacter, };
            }
        }

        private void CharacterClick(int position)
        {
            var currentState = WordCandidate[position].State;
            var nStates = Enum.GetValues(typeof(CharacterState)).Length;
            WordCandidate[position].State = (int)currentState == nStates - 1 ? CharacterState.WrongCharacter : (CharacterState)(int)currentState + 1;
        }
    }
}