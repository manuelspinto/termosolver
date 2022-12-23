using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using TermoSolver.Models;

namespace TermoSolver.Services.Solver
{
    public class WordSolver : IWordSolver
    {
        private const decimal RandomMatchThresholdPercentage = 0.95m;

        public string? GetBestWord(IEnumerable<WordScore> words, WordFilter filter)
        {
            var filteredWords = FilterDictionary(words, filter);
            return filteredWords.OrderByDescending(w => w.Score).FirstOrDefault()?.Word;
        }

        public string? GetNextWord(IEnumerable<WordScore> words, WordFilter filter)
        {
            var filteredWords = FilterDictionary(words, filter);
            return filteredWords.Any() ? BestMatch(filteredWords) : null;
        }

        public IEnumerable<WordScore>? GetNextWords(IEnumerable<WordScore> words, WordFilter filter)
        {
            var filteredWords = FilterDictionary(words, filter);
            return filteredWords.OrderByDescending(x => x.Score);
        }

        private string? BestMatch(IEnumerable<WordScore> filteredWords)
        {
            int lowScore = filteredWords.OrderBy(x => x.Score).First().Score;
            int highScore = filteredWords.OrderByDescending(x => x.Score).First().Score;
            int range = highScore - lowScore;

            decimal minimumScore = lowScore + (range * RandomMatchThresholdPercentage);

            var bestMatches = filteredWords.Where(x => x.Score >= minimumScore);
            var count = bestMatches.Count();

            Random r = new Random();
            var randomIndex = r.Next(0, count);

            return bestMatches.ElementAt(randomIndex).Word;
        }

        public void IterateFilter(WordFilter filter, WordState[] wordState)
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
                        if(filter.MisplacedChars.Where(x => x.Character == charState.Character).First().Frequency == 0)
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
                    if(!filter.ProhibitedChars
                        .Select(x => x.Character)
                        .Contains(charState.Character))
                    {
                        filter.ProhibitedChars.Add(new GroupChar(charState.Character, 1));
                    }

                    if(filter.MisplacedChars.Any(x => x.Character == charState.Character))
                    {
                        filter.MisplacedChars.Where(x => x.Character == charState.Character).First().Position.Add(i);
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

        private static IEnumerable<WordScore> FilterDictionary(IEnumerable<WordScore> words, WordFilter filter)
            => words.Distinct().Where(ws => FilterWord(ws.Word, filter));

        private static bool FilterWord(string word, WordFilter filter)
        {
            if (word == "UREIA")
            {
                string a = word;
            }
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
                .Select(c => new { c.Character, Frequency = c.Frequency + filter.AllowedChars.Count(a => a == c.Character) });

            foreach (var cg in charGroup)
            {
                if (word.Count(w => w == cg.Character) < cg.Frequency)
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
