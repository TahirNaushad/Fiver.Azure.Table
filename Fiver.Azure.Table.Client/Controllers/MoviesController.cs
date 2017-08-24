using Fiver.Azure.Table.Client.Lib;
using Fiver.Azure.Table.Client.Models.Movies;
using Fiver.Azure.Table.Client.OtherLayers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fiver.Azure.Table.Client.Controllers
{
    [Route("movies")]
    public class MoviesController : BaseController
    {
        private readonly IMovieService service;

        public MoviesController(IMovieService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var model = await service.GetMovies();

            var outputModel = ToOutputModel(model);
            return Ok(outputModel);
        }

        [HttpGet("{releaseYear}/{title}", Name = "GetMovie")]
        public async Task<IActionResult> Get(string releaseYear, string title)
        {
            var model = await service.GetMovie(releaseYear, title);
            if (model == null)
                return NotFound();

            var outputModel = ToOutputModel(model);
            return Ok(outputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]MovieInputModel inputModel)
        {
            if (inputModel == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return Unprocessable(ModelState);

            var model = ToDomainModel(inputModel);
            await service.AddMovie(model);

            var outputModel = ToOutputModel(model);
            return CreatedAtRoute("GetMovie", 
                new { releaseYear = outputModel.ReleaseYear, title = outputModel.Title }, 
                outputModel);
        }

        [HttpPut("{releaseYear}/{title}")]
        public async Task<IActionResult> Update(string releaseYear, string title, 
            [FromBody]MovieInputModel inputModel)
        {
            if (inputModel == null || 
                releaseYear != inputModel.ReleaseYear.ToString() ||
                title != inputModel.Title)
                return BadRequest();

            if (!await service.MovieExists(releaseYear, title))
                return NotFound();

            if (!ModelState.IsValid)
                return new UnprocessableObjectResult(ModelState);

            var model = ToDomainModel(inputModel);
            await service.UpdateMovie(model);

            return NoContent();
        }
        
        [HttpDelete("{releaseYear}/{title}")]
        public async Task<IActionResult> Delete(string releaseYear, string title)
        {
            if (!await service.MovieExists(releaseYear, title))
                return NotFound();

            await service.DeleteMovie(releaseYear, title);

            return NoContent();
        }

        #region " Mappings "

        private MovieOutputModel ToOutputModel(Movie model)
        {
            return new MovieOutputModel
            {
                Title = model.Title,
                ReleaseYear = model.ReleaseYear,
                Summary = model.Summary,
                LastReadAt = DateTime.Now
            };
        }

        private List<MovieOutputModel> ToOutputModel(List<Movie> model)
        {
            return model.Select(item => ToOutputModel(item))
                        .ToList();
        }

        private Movie ToDomainModel(MovieInputModel inputModel)
        {
            return new Movie
            {
                PartitionKey = inputModel.ReleaseYear.ToString(),
                RowKey = inputModel.Title,

                Title = inputModel.Title,
                ReleaseYear = inputModel.ReleaseYear,
                Summary = inputModel.Summary
            };
        }

        private MovieInputModel ToInputModel(Movie model)
        {
            return new MovieInputModel
            {
                Title = model.Title,
                ReleaseYear = model.ReleaseYear,
                Summary = model.Summary
            };
        }
        
        #endregion
    }
}
