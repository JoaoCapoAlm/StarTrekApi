using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TMDB.Enum;
using TMDB.Model;

namespace TMDB
{
    public class TmdbAPI
    {
        private static readonly string _baseUrl = "https://api.themoviedb.org/3";
        private static readonly string _apiKey = "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiI3NzFkN2UwZWRhZTczYzM1YmJkNzc2Y2FiNzRkMmMwZiIsInN1YiI6IjY1NDEwZTViNmNhOWEwMDE0ZjBlMTg2MCIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ._LJ1BOkQLeZN6croVSzN5qrsK-5flY70BLZ7P0Kqe8Y";
        private static HttpClient GenerateClient()
        {
            var _client = new HttpClient
            {
                BaseAddress = new Uri($"{_baseUrl}/"),
                DefaultRequestVersion = HttpVersion.Version30,
                DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher,
                Timeout = TimeSpan.FromMinutes(1)
            };
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );
            _client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-US"));
            _client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("utf-8"));

            return _client;
        }

        public async Task<TvGetDetails?> SearchSerie(int serieId)
        {
            var response = new HttpResponseMessage();

            using (var _client = GenerateClient())
            {
                response = await _client.GetAsync($"{TmdbTypeEnum.tv}/{serieId}");
            }

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<TvGetDetails>();

            throw new ApplicationException();
        }

        public async Task<MovieGetDetails?> SearchMovie(int movieId)
        {
            var response = new HttpResponseMessage();

            using (var _client = GenerateClient())
            {
                response = await _client.GetAsync($"{TmdbTypeEnum.movie}/{movieId}");
            }

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<MovieGetDetails>();

            throw new ApplicationException();
        }
    }
}
