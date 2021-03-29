using Newtonsoft.Json;

namespace ShowRenamer.Models
{
    public class TmdbEpisodeResultModel
    {
        [JsonProperty("episode_number")]
        public int Number { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("season_number")]
        public int SeasonNumber { get; set; }
    }
}
