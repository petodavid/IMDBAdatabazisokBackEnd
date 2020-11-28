using System;
namespace IMDBAdatabazisok
{
    public class MovieImage
    {

        public String kepURL { get; set; }


        internal AppDb Db { get; set; }

        public MovieImage()
        {
        }
        internal MovieImage(AppDb db)
        {
            Db = db;
        }
    }
}
