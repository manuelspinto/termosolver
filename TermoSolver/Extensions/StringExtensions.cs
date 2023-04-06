using TermoSolver.Services.Solver.Models;

namespace TermoSolver.Extensions
{
    public static class StringExtensions
    {
        public static WordState[]? ToInitialWordState(this string? str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return null;
            }

            WordState[]? wordState = new WordState[str.Length];

            for (int i = 0; i < wordState.Length; i++)
            {
                wordState[i] = new WordState() { Character = str[i], State = CharacterState.WrongCharacter };
            }

            return wordState;
        }
    }
}
