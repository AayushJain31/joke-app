using JokesApp.Models;
using JokesApp.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace JokesApp.Controllers
{
    public class JokeController : Controller
    {
        private const string baseURL = "https://icanhazdadjoke.com/";
        public JokeService _jokeService;

        public JokeController()
        {
            _jokeService = new JokeService();
        }

        public async Task<IActionResult> Index()
        {
            JokeModel joke = null;
            try
            {
                using (var client = new HttpClient())
                {
                    //client.BaseAddress = new Uri(baseURL);
                    client.DefaultRequestHeaders.Add("Accept", "application/json");

                    HttpResponseMessage response = await client.GetAsync(baseURL);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        if (json != null)
                        {
                            var jokeResponse = JsonConvert.DeserializeObject<JokeModel>(json);

                            if (jokeResponse != null)
                            {
                                return View(jokeResponse);
                            }
                            else
                            {
                                joke = new JokeModel() { Joke = "No joke found. Please try again." };
                            }
                        }
                    }
                    else
                    {
                        joke = new JokeModel() { Joke = "Failed to fetch a joke, Please try again" };
                    }
                }
                if (joke == null)
                {
                    joke = new JokeModel() { Joke = "Please click on the New Joke button to view a joke." };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                joke = new JokeModel() { Joke = $"Joke request failed : {ex.Message}." };
            }

            return View(joke);
        }

        public async Task<IActionResult> SearchJokes(string searchParam)
        {
            List<JokeModel> jokes = new List<JokeModel>();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseURL);
                    client.DefaultRequestHeaders.Add("Accept", "application/json");

                    HttpResponseMessage response = await client.GetAsync($"/search?term={searchParam}&limit=30");

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        var jokeResponse = JsonConvert.DeserializeObject<Jokes>(json);
                        
                        if(jokeResponse != null && jokeResponse.Results.Count > 0) {
                            _jokeService.FormatandGroupJokesByLength(jokeResponse.Results, searchParam);
                            return PartialView("_JokePartial", jokeResponse.Results);
                        }
                        else
                        {
                            jokes.Add(new JokeModel() { Joke = "No joke(s) found. Please modify your search." });
                        }
                        
                    }
                    else
                    {
                        jokes.Add(new JokeModel() { Joke = "Failed to fetch a joke, Please try again" });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                jokes.Add(new JokeModel() { Joke = $"Joke request failed : {ex.Message}." });
            }

            return PartialView("_JokePartial", jokes);
        }
    }
}
