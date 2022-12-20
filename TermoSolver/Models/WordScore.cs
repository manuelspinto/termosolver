namespace TermoSolver.Models
{
    public class WordScore
    {
        public WordScore(string word, int score)
        {
            Word = word;
            Score = score;
        }

        public string Word { get; set; }
        public int Score { get; set; }
    }

    public class WordScoreComparer : IEqualityComparer<WordScore>
    {
        // Products are equal if their names and product numbers are equal.
        public bool Equals(WordScore x, WordScore y)
        {

            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether the products' properties are equal.
            return x.Word == y.Word && x.Score == y.Score;
        }

        // If Equals() returns true for a pair of objects
        // then GetHashCode() must return the same value for these objects.

        public int GetHashCode(WordScore product)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(product, null)) return 0;

            //Get hash code for the Name field if it is not null.
            int hashWord = product.Word == null ? 0 : product.Word.GetHashCode();

            //Get hash code for the Code field.
            int hashScore = product.Score.GetHashCode();

            //Calculate the hash code for the product.
            return hashWord ^ hashScore;
        }
    }
}
