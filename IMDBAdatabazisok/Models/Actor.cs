using System;
namespace IMDBAdatabazisok.Models
{
    public class Actor
    {

        public int szinesz_id { get; set; }
        public String neve { get; set; }
        public String kor { get; set; }


        internal AppDb Db { get; set; }

        public Actor()
        {
        }
        internal Actor(AppDb db)
        {
            Db = db;
        }
    }
}
