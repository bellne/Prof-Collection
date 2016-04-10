using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DVDLibrary.Models
{
    public class AddMovieVM
    {
        public Movie Movie { get; set; }
        public List<SelectListItem> RatingList { get; set; }
        public List<SelectListItem> StarList { get; set; }

        public AddMovieVM()
        {
            RatingList = new List<SelectListItem>
            {
                new SelectListItem {Text="G", Value="1" },
                new SelectListItem {Text="PG", Value="2" },
                new SelectListItem {Text="PG-13", Value="3" },
                new SelectListItem {Text="R", Value="4" },
                new SelectListItem {Text="NC-17", Value="5" },
                new SelectListItem {Text="NR", Value="6" }
            };

            StarList = new List<SelectListItem>
            {
                new SelectListItem {Text="0", Value="1" },
                new SelectListItem {Text="1", Value="2" },
                new SelectListItem {Text="2", Value="3" },
                new SelectListItem {Text="3", Value="4" },
                new SelectListItem {Text="4", Value="5" },
                new SelectListItem {Text="5", Value="6" }
            };
        }
    }
}