using System;
using System.Collections.Generic;

namespace IMDBAdatabazisok
{
    public class Movie
    {
        public int film_id { get; set; }
        public String kategoria { get; set; }
        public int ertekeles { get; set; }
        public String szinesz { get; set; }
        public String nev { get; set; }
        public String leiras { get; set; }
        public String boritoKepURL { get; set; }
        public List<MovieImage> filmKepek { get; set; }

        internal AppDb Db { get; set; }

        public Movie()
        {
        }
        internal Movie(AppDb db)
        {
            Db = db;
        }
    }
}
