using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IMDBAdatabazisok.Controllers
{
    [Route("api/[controller]")]
    public class GenresController : Controller
    {
        public AppDb Db { get; }
        public GenresController(AppDb db)
        {
            Db = db;
        }

        // GET: api/values
        [HttpGet]
        public async Task<IActionResult> getGenres()
        {
            await Db.Connection.OpenAsync();
            var query = new GenreQuery(Db);
            var result = await query.getGenres();
            return new OkObjectResult(result);
        }

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
