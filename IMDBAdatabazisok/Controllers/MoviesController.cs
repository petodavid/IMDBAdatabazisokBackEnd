using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IMDBAdatabazisok.Controllers
{
    [Route("api/[controller]")]

    public class MoviesController : ControllerBase
    {

        public AppDb Db { get; }
        public MoviesController(AppDb db)
        {
            Db = db;
        }

        // GET: api/values
        [HttpGet]
        public async Task<IActionResult> GetMovies()
        {
            await Db.Connection.OpenAsync();
            var moviesQuery = new MovieQuery(Db);
            var movieImagesQuery = new MovieImageQuery(Db);
            var movies = await moviesQuery.getMovies();
            foreach (var movie in movies)
            {
                movie.filmKepek = await movieImagesQuery.getMovieImagesById(movie.film_id);
            }

            return new OkObjectResult(movies);
        }

        // GET: api/values
        [HttpGet("top10")]
        public async Task<IActionResult> GetTop10Movies()
        {
            await Db.Connection.OpenAsync();
            var moviesQuery = new MovieQuery(Db);
            var movieImagesQuery = new MovieImageQuery(Db);
            var movies = await moviesQuery.getTop10Movies();
            foreach (var movie in movies)
            {
                movie.filmKepek = await movieImagesQuery.getMovieImagesById(movie.film_id);
            }

            return new OkObjectResult(movies);
        }


        // POST api/values
        [HttpPost("new")]
        public async Task<IActionResult> AddToMovies([FromBody] Movie movie)
        {
            await Db.Connection.OpenAsync();
            var query = new MovieQuery(Db);
            var result = await query.addNewMovie(movie);
            return new OkObjectResult(result);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovieById(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new MovieQuery(Db);
            var result = await query.delelteMovieById(id);
            return new OkObjectResult(result);
        }

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}



    }
}
