using Newtonsoft.Json;
using System.Collections.Generic;

namespace ShowRenamer.Models
{
    public class TmdbShowResultModel
    {
        [JsonProperty("seasons")]
        public IEnumerable<TmdbSeasonResultModel> Seasons { get; set; }
    }
}
