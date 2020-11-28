using System;
namespace IMDBAdatabazisok
{
    public class Genre
    {
        public int kategoria_id { get; set; }
        public String kategoriaNev { get; set; }

        internal AppDb Db { get; set; }

        public Genre()
        {
        }
        internal Genre(AppDb db)
        {
            Db = db;
        }
    }
}
