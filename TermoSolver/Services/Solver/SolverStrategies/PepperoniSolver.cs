using TermoSolver.Models;
using TermoSolver.Services.Solver.Models;

namespace TermoSolver.Services.Solver.SolverStrategies
{
    public class PepperoniSolver : WordSolver
    {
        public override void IterateFilter(WordFilter filter, WordState[] wordState)
        {
            for (int i = 0; i < wordState.Length; i++)
            {
                var charState = wordState[i];
                if (charState.State == CharacterState.RightPosition)
                {
                    if (filter.MisplacedChars.Any(x => x.Character == charState.Character) &&
                        filter.AllowedChars[i] == '-')
                    {
                        filter.MisplacedChars.Where(x => x.Character == charState.Character).First().Frequency--;
                        if (filter.MisplacedChars.Where(x => x.Character == charState.Character).First().Frequency == 0)
                            filter.MisplacedChars.RemoveAll(x => x.Character == charState.Character);
                    }

                    filter.AllowedChars[i] = charState.Character;
                }
                else
                {
                    filter.AllowedChars[i] = '-';
                }
            }

            for (int i = 0; i < wordState.Length; i++)
            {
                var charState = wordState[i];
                if (charState.State == CharacterState.WrongCharacter)
                {
                    if (!filter.ProhibitedChars
                        .Select(x => x.Character)
                        .Contains(charState.Character))
                    {
                        filter.ProhibitedChars.Add(new GroupChar(charState.Character, 1));
                    }

                    if (filter.MisplacedChars.Any(x => x.Character == charState.Character))
                    {
                        filter.MisplacedChars.Where(x => x.Character == charState.Character).First().Position.Add(i);
                    }
                }

                if (charState.State == CharacterState.WrongPosition)
                {
                    if (filter.MisplacedChars.Any(x => x.Character == charState.Character))
                    {
                        filter.MisplacedChars.Where(x => x.Character == charState.Character).FirstOrDefault()?
                            .Position.Add(i);
                    }
                    else
                    {
                        filter.MisplacedChars.Add(
                            new PositionalChar()
                            {
                                Character = charState.Character,
                                Position = new List<int>() { i }
                            });
                    }
                }
            }

            for (int i = 0; i < wordState.Length; i++)
            {
                var charState = wordState[i];
                if (
                    (
                        filter.AllowedChars.Contains(charState.Character) ||
                        filter.MisplacedChars.Any(c => c.Character == charState.Character)
                    ) &&
                    filter.ProhibitedChars.Where(c => c.Character == charState.Character).Any())
                {
                    filter.ProhibitedChars.Where(c => c.Character == charState.Character).First().Frequency =
                        filter.ProhibitedChars.Count(c => c.Character == charState.Character) +
                        filter.AllowedChars.Count(c => c == charState.Character) +
                        filter.MisplacedChars.Count(c => c.Character == charState.Character);
                }
            }

            filter.MisplacedChars.ForEach(c => c.Frequency = wordState.Count(ws => ws.Character == c.Character && ws.State == CharacterState.WrongPosition));
        }
    }
}
