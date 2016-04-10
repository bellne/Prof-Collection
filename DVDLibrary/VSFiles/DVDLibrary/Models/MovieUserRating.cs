using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DVDLibrary.Models
{
    public class MovieUserRating
    {
        public int UserRatingID { get; set; }
        public int UserRating { get; set; }
        public string UserRatingDescription { get; set; }
    }
}