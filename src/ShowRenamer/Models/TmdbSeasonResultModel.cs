using Newtonsoft.Json;

namespace ShowRenamer.Models
{
    public class TmdbSeasonResultModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("season_number")]
        public int Number { get; set; }
    }
}
