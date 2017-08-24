using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fiver.Azure.Table.Client.OtherLayers
{
    public interface IMovieService
    {
        Task AddMovie(Movie item);
        Task DeleteMovie(string releaseYear, string title);
        Task<Movie> GetMovie(string releaseYear, string title);
        Task<List<Movie>> GetMovies();
        Task<bool> MovieExists(string releaseYear, string title);
        Task UpdateMovie(Movie item);
    }
}