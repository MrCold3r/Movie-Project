using ASP_MVC_Movie.Models;

namespace ASP_MVC_Movie.Interfaces
{
    public interface ISearchService
    {
        // All method name says what it can

        Task<IEnumerable<object>> SearchMoviesAsync(string query);
    }
}
