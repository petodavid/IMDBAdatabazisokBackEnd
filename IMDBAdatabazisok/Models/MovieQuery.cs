using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;
namespace IMDBAdatabazisok
{
    public class MovieQuery
    {
        public AppDb Db { get; }

        public MovieQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<List<Movie>> getMovies()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"select * from ((Filmek
            inner join Kategoriak K on K.kategoria_id = Filmek.kategoria
            inner join Szineszek S on Filmek.szinesz = S.szinesz_id
            ))";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task<List<Movie>> getTop10Movies()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"select * from ((Filmek
            inner join Kategoriak K on K.kategoria_id = Filmek.kategoria
            inner join Szineszek S on Filmek.szinesz = S.szinesz_id
            )) order by ertekeles DESC  LIMIT 10";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task<int> delelteMovieById(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"delete from Kepek where  Kepek.film_id = @id;
            delete from Filmek where Filmek.film_id =@id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            return await cmd.ExecuteNonQueryAsync();
        }

        public async Task<int> addNewMovie(Movie movie)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var url in movie.filmKepek)
            {
                sb.Append("(LAST_INSERT_ID()," + "'" + url.kepURL + "'" + "),");
            }

            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"begin;
            INSERT INTO Filmek (nev, kategoria, ertekeles, leiras, boritokepurl, szinesz)
            VALUES(@nev, @kategoria, @ertekeles,@leiras,@boritokepurl,@szinesz);
            INSERT INTO Kepek (film_id, URL)
            VALUES" + sb.ToString().Remove(sb.ToString().Length - 1) + ";" + System.Environment.NewLine +
            " commit;";
            Console.WriteLine(cmd.CommandText);
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@nev",
                DbType = DbType.String,
                Value = movie.nev,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@kategoria",
                DbType = DbType.Int32,
                Value = movie.kategoria,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ertekeles",
                DbType = DbType.Int32,
                Value = movie.ertekeles,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@leiras",
                DbType = DbType.String,
                Value = movie.leiras,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@boritokepurl",
                DbType = DbType.String,
                Value = movie.boritoKepURL,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@szinesz",
                DbType = DbType.Int32,
                Value = movie.szinesz,
            });
            return await cmd.ExecuteNonQueryAsync();

        }

        public async Task<int> updateMovie(int id, Movie movie)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var url in movie.filmKepek)
            {
                sb.Append("(" + id + " , " + "'" + url.kepURL + "'" + "),");
            }

            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"update Filmek set nev = @nev, kategoria = @kategoria, ertekeles = @ertekeles, leiras = @leiras, boritoKepURL= @boritoKepUrl, szinesz = @szinesz  where Filmek.film_id = @filmId;
            delete from Kepek where Kepek.film_id = @filmId;
            INSERT INTO Kepek (film_id, URL)
            VALUES" + sb.ToString().Remove(sb.ToString().Length - 1) + ";";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@nev",
                DbType = DbType.String,
                Value = movie.nev,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@kategoria",
                DbType = DbType.Int32,
                Value = movie.kategoria,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ertekeles",
                DbType = DbType.Int32,
                Value = movie.ertekeles,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@leiras",
                DbType = DbType.String,
                Value = movie.leiras,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@boritoKepUrl",
                DbType = DbType.String,
                Value = movie.boritoKepURL,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@szinesz",
                DbType = DbType.Int32,
                Value = movie.szinesz,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@filmId",
                DbType = DbType.Int32,
                Value = id,
            });
            Console.WriteLine(cmd.CommandText);
            return await cmd.ExecuteNonQueryAsync();
        }

        private async Task<List<Movie>> ReadAllAsync(DbDataReader reader)
        {
            var movies = new List<Movie>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var movie = new Movie(Db)
                    {
                        nev = reader.GetString(0),
                        film_id = reader.GetInt32(1),
                        kategoria = reader.GetString(7),
                        ertekeles = reader.GetInt32(3),
                        leiras = reader.GetString(4),
                        boritoKepURL = reader.GetString(5),
                        szinesz = reader.GetString(10),
                    };
                    movies.Add(movie);
                }
            }
            return movies;
        }
    }
}
