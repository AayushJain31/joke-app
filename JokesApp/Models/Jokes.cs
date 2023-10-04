namespace JokesApp.Models
{
    public class Jokes
    {
        public Jokes()
        {
            Results = new List<JokeModel>();
        }

        public IList<JokeModel> Results { get; set; }
    }
}
