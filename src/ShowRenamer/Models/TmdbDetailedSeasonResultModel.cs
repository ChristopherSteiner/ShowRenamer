using Newtonsoft.Json;
using System.Collections.Generic;

namespace ShowRenamer.Models
{
    public class TmdbDetailedSeasonResultModel
    {
        [JsonProperty("episodes")]
        public IEnumerable<TmdbEpisodeResultModel> Episodes { get; set; }
    }
}
