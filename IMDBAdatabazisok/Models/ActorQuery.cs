using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;

namespace IMDBAdatabazisok.Models
{
    public class ActorQuery
    {

        public AppDb Db { get; }

        public ActorQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<List<Actor>> getActors()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"select * from Szineszek ";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        private async Task<List<Actor>> ReadAllAsync(DbDataReader reader)
        {
            var actors = new List<Actor>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var actor = new Actor(Db)
                    {
                        szinesz_id = reader.GetInt16(0),
                        neve = reader.GetString(1),
                        kor = reader.GetString(2)

                    };
                    actors.Add(actor);
                }
            }
            return actors;
        }
    }
}
