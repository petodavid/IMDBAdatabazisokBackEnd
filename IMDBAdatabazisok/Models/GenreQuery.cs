using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;
namespace IMDBAdatabazisok
{
    public class GenreQuery
    {
        public AppDb Db { get; }

        public GenreQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<List<Genre>> getGenres()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"select * from Kategoriak ";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        private async Task<List<Genre>> ReadAllAsync(DbDataReader reader)
        {
            var genres = new List<Genre>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var genre = new Genre(Db)
                    {
                        kategoriaNev = reader.GetString(0),
                        kategoria_id = reader.GetInt16(1),


                    };
                    genres.Add(genre);
                }
            }
            return genres;
        }
    }
}
