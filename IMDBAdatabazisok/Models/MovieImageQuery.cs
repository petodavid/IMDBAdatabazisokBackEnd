using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;
namespace IMDBAdatabazisok
{
    public class MovieImageQuery
    {
        public AppDb Db { get; }

        public MovieImageQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<List<MovieImage>> getMovieImagesById(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"select URL from Kepek where Kepek.film_id=@id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        private async Task<List<MovieImage>> ReadAllAsync(DbDataReader reader)
        {
            var movieImages = new List<MovieImage>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var movieImage = new MovieImage(Db)
                    {
                        kepURL = reader.GetString(0),

                    };
                    movieImages.Add(movieImage);
                }
            }
            return movieImages;
        }
    }
}
