using Newtonsoft.Json;
using PROJECT_ES.Data;

namespace PROJECT_ES.Service;

public class MovieService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public MovieService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<Movie> GetMovieAsync(string title)
    {
        var apiKey = _configuration.GetValue<string>("OMDbApiKey");
        var url = $"http://www.omdbapi.com/?apikey={apiKey}&t={title}&plot=full";

        var response = await _httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var movie = JsonConvert.DeserializeObject<Movie>(content);

            return movie;
        }

        return null;
    }
}