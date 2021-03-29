using Newtonsoft.Json;

namespace ShowRenamer.Models
{
    public class TmdbSearchShowResultModel
    {
        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }

        [JsonIgnore]
        public string PosterUrl
        {
            get => $"https://image.tmdb.org/t/p/w500/{PosterPath}";
        }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("overview")]
        public string Overview { get; set; }
    }
}
