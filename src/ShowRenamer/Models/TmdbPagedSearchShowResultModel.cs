using Newtonsoft.Json;
using System.Collections.Generic;

namespace ShowRenamer.Models
{
    public class TmdbPagedSearchShowResultModel
    {
        [JsonProperty("page")]
        public int PageIndex { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPageCount { get; set; }

        [JsonProperty("total_results")]
        public int TotalResultCount { get; set; }

        [JsonProperty("results")]
        public IEnumerable<TmdbSearchShowResultModel> SearchResults { get; set; }

    }
}
