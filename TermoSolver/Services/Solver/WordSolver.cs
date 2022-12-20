using System.Security.Cryptography.X509Certificates;
using TermoSolver.Models;

namespace TermoSolver.Services.Solver
{
    public class WordSolver : IWordSolver
    {
        public string? GetNextWord(IEnumerable<WordScore> words, WordFilter filter)
        {
            var filteredWords = FilterDictionary(words, filter);
            return filteredWords.OrderByDescending(x => x.Score).FirstOrDefault()?.Word;
        }

        public IEnumerable<WordScore>? GetNextWords(IEnumerable<WordScore> words, WordFilter filter)
        {
            var filteredWords = FilterDictionary(words, filter);
            return filteredWords.OrderByDescending(x => x.Score);
        }

        public void IterateFilter(WordFilter filter, WordState[] wordState)
        {
            for(int i = 0; i < wordState.Length; i++)
            {
                var charState = wordState[i];
                if (charState.State == CharacterState.RightPosition)
                {
                    if(filter.MisplacedChars.Any(x => x.Character == charState.Character) &&
                        filter.AllowedChars[i] == '-')
                    {
                        filter.MisplacedChars.RemoveAll(x => x.Character == charState.Character);
                    }

                    filter.AllowedChars[i] = charState.Character;
                }
                else
                {
                    filter.AllowedChars[i] = '-';
                }

                if(charState.State == CharacterState.WrongCharacter)
                {
                    if(!filter.ProhibitedChars
                        .Select(x => x.Character)
                        .Contains(charState.Character))
                    {
                        filter.ProhibitedChars.Add(new GroupChar(charState.Character, 1));
                    }
                    else
                    {
                        if (filter.AllowedChars.Contains(charState.Character))
                        {
                            filter.ProhibitedChars.Where(c => c.Character == charState.Character).First().Frequency++;
                        }
                    }
                }

                if(charState.State == CharacterState.WrongPosition)
                {
                    if(filter.MisplacedChars.Any(x => x.Character == charState.Character))
                    {
                        filter.MisplacedChars.Where(x => x.Character == charState.Character).FirstOrDefault()?
                            .Position.Add(i);
                    } else
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
        }

        private static IEnumerable<WordScore> FilterDictionary(IEnumerable<WordScore> words, WordFilter filter)
            => words.Distinct().Where(ws => FilterWord(ws.Word, filter));

        private static bool FilterWord(string word, WordFilter filter)
        {
            for (int i = 0; i < word.Length; i++)
            {
                if (filter.ProhibitedChars.Select(c => c.Character).Contains(word[i]))
                {
                    if (word.Count(w => w == word[i]) >= filter.ProhibitedChars.First(c => c.Character == word[i]).Frequency)
                    {
                        return false;
                    }
                }
                if (filter.AllowedChars[i] != '-' && filter.AllowedChars[i] != word[i])
                {
                    return false;
                }
            }

            var charGroup = filter.MisplacedChars
                .GroupBy(cp => cp.Character)
                .Select(c => new { Character = c.Key, Frequency = c.Count() + filter.AllowedChars.Count(a => a == c.Key) });

            foreach (var cg in charGroup)
            {
                if (word.Count(w => w == cg.Character) != cg.Frequency)
                {
                    return false;
                }
            }

            foreach (var cp in filter.MisplacedChars)
            {
                foreach (var p in cp.Position)
                {
                    if (word[p] == cp.Character)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
