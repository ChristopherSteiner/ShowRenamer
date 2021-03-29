using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ShowRenamer.Configuration;
using ShowRenamer.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace ShowRenamer.Services.Tmdb
{
    public class TmdbService : ITmdbService
    {
        private readonly HttpClient httpClient;
        private readonly TmdbConfig tmdbOptions;

        public TmdbService(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            tmdbOptions = configuration.GetSection("Tmdb").Get<TmdbConfig>();
        }

        public async Task<TmdbPagedSearchShowResultModel> SearchTvShowAsync(string searchQuery)
        {
            string response =
                await httpClient.GetStringAsync($"{tmdbOptions.ApiUrl}search/tv?query={HttpUtility.UrlEncode(searchQuery)}")
                .ConfigureAwait(false);
            return JsonConvert.DeserializeObject<TmdbPagedSearchShowResultModel>(response);
        }

        public async Task<IEnumerable<TmdbEpisodeResultModel>> GetAllEpisodes(int showId)
        {
            string response =
                await httpClient.GetStringAsync($"{tmdbOptions.ApiUrl}tv/{showId}")
                .ConfigureAwait(false);
            TmdbShowResultModel show = JsonConvert.DeserializeObject<TmdbShowResultModel>(response);
            
            if (show != null && show.Seasons != null)
            {
                List<TmdbEpisodeResultModel> episodes = new List<TmdbEpisodeResultModel>();
                foreach (TmdbSeasonResultModel season in show.Seasons)
                {
                    string seasonResponse =
                        await httpClient.GetStringAsync($"{tmdbOptions.ApiUrl}tv/{showId}/season/{season.Number}")
                        .ConfigureAwait(false);
                    TmdbDetailedSeasonResultModel detailedSeason = JsonConvert.DeserializeObject<TmdbDetailedSeasonResultModel>(seasonResponse);
                    episodes.AddRange(detailedSeason.Episodes);
                }

                return episodes;
            }

            return Enumerable.Empty<TmdbEpisodeResultModel>();
        }
    }
}
