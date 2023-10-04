using JokesApp.Models;
using System.Text.RegularExpressions;

namespace JokesApp.Services
{
    public class JokeService
    {
        //Returns the count of words in the string
        public int CountWords(string s)
        {
            MatchCollection collection = Regex.Matches(s, @"[\S]+");
            return collection.Count;
        }

        public void FormatandGroupJokesByLength(IList<JokeModel> jokes, string searchParam)
        {
            foreach (JokeModel joke in jokes)
            {
                if(Regex.IsMatch(joke.Joke, searchParam))
                {
                    //Emphasize the matching search term ( using bold tag(<b>) and UpperCase)
                    joke.Joke = Regex.Replace(joke.Joke, searchParam, $"<b>{searchParam.ToUpper()}</b>");
                }

                int wordCount = CountWords(joke.Joke);

                // Grouping the jokes by length (S-small, M-medium and L-large)
                if(wordCount < 10) {
                    joke.Length = 'S';
                }
                else if(wordCount >= 10 && wordCount < 20)
                {
                    joke.Length = 'M';
                }
                else
                {
                    joke.Length = 'L';
                }
            }
            
        }
    }
}
