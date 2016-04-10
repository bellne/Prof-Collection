using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DVDLibrary.Models
{
    public class Movie
    {
        public int MovieID { get; set; }
        public string Title { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public MovieMPAARating MovieMPAARating { get; set; }
        public int MPAARatingId { get; set; }
        public string DirectorName { get; set; }
        public string Studio { get; set; }
        public MovieUserRating MovieUserRating { get; set; }
        public int UserRatingId { get; set; }
        public string UserNotes { get; set; }
        public string Actors { get; set; }
        public string BorrowerName { get; set; }
        public DateTime? DateBorrowed { get; set; }
        public DateTime? DateReturned { get; set; }
    }
}