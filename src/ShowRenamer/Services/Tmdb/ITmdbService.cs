using ShowRenamer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShowRenamer.Services.Tmdb
{
    public interface ITmdbService
    {
        Task<TmdbPagedSearchShowResultModel> SearchTvShowAsync(string searchQuery);

        Task<IEnumerable<TmdbEpisodeResultModel>> GetAllEpisodes(int showId);
    }
}
