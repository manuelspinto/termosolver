using TermoSolver.Models;
using TermoSolver.Services.Solver.Models;

namespace TermoSolver.Services.Solver
{
    public abstract class WordSolver : IWordSolver
    {
        protected const decimal RandomMatchThresholdPercentage = 0.95m;

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

        public string? GetInitialWord(IEnumerable<WordScore> words)
        {
            return GetNextWord(words, new WordFilter());
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

            decimal minimumScore = lowScore + range * RandomMatchThresholdPercentage;

            var bestMatches = filteredWords.Where(x => x.Score >= minimumScore);
            var count = bestMatches.Count();

            Random r = new Random();
            var randomIndex = r.Next(0, count);

            return bestMatches.ElementAt(randomIndex).Word;
        }

        public abstract void IterateFilter(WordFilter filter, WordState[] wordState);

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
