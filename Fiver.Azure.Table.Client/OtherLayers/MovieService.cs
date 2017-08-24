using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fiver.Azure.Table.Client.OtherLayers
{
    public class MovieService : IMovieService
    {
        private readonly IAzureTableStorage<Movie> repository;

        public MovieService(IAzureTableStorage<Movie> repository)
        {
            this.repository = repository;
        }

        public async Task<List<Movie>> GetMovies()
        {
            return await this.repository.GetList();
        }

        public async Task<Movie> GetMovie(string releaseYear, string title)
        {
            return await this.repository.GetItem(releaseYear, title);
        }

        public async Task AddMovie(Movie item)
        {
            await this.repository.Insert(item); 
        }

        public async Task UpdateMovie(Movie item)
        {
            await this.repository.Update(item);
        }

        public async Task DeleteMovie(string releaseYear, string title)
        {
            await this.repository.Delete(releaseYear, title);
        }

        public async Task<bool> MovieExists(string releaseYear, string title)
        {
            return await this.repository.GetItem(releaseYear, title) != null;
        }
    }
}
