using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace DVDLibrary.Models
{
    public class MovieRepository : IMovieRepository
    {
        public static List<Movie> movies;

        public void Delete(int id)
        {
            var p = new DynamicParameters();
            p.Add("MovieId", id);
            using (SqlConnection sqlConnection = new SqlConnection(Settings.ConnectionString))
            {
                string sqlQuery = "DELETE FROM Movie WHERE MovieId = @MovieId";
                sqlConnection.Execute(sqlQuery, new {MovieId = id });
            }
        }

        public void Edit(Movie movie)
        {
            using (SqlConnection sqlConnection = new SqlConnection(Settings.ConnectionString))
            {
                string sqlQuery = "UPDATE Movie SET Title = @Title, ReleaseDate = @ReleaseDate, MPAARatingID = @MPAARatingID, DirectorName = @DirectorName, " +
                                "Studio = @Studio, UserRatingID = @UserRatingID, UserNotes = @UserNotes, Actors = @Actors, BorrowerName = @BorrowerName, " +
                                "DateBorrowed = @DateBorrowed, DateReturned = @DateReturned " +
                                "WHERE MovieID = @MovieID";
                try
                {
       // This was the only way to get around an error I kept getting saying that i must declare a scalar value for "@Title" and all the other variables
                    sqlConnection.Execute(sqlQuery, new
                    {
                        MovieID = movie.MovieID,
                        Title = movie.Title,
                        ReleaseDate = movie.ReleaseDate,
                        MPAARatingID = Int32.Parse(movie.MPAARatingId.ToString()),
                        DirectorName = movie.DirectorName,
                        Studio = movie.Studio,
                        UserRatingID = Int32.Parse(movie.UserRatingId.ToString()),
                        UserNotes = movie.UserNotes,
                        Actors = movie.Actors,
                        BorrowerName = movie.BorrowerName,
                        DateBorrowed = movie.DateBorrowed,
                        DateReturned = movie.DateReturned
                    });
                }
                catch
                {
                    
                }
            }
        }

        public Movie Get(int MovieId)
        {
            return  movies.FirstOrDefault(m=>m.MovieID==MovieId);

            ////////////////////////Nate's Get//////////////////////////////////////////////////////
            //Movie Fullmovie;
            //List<MovieMPAARating> mpaaRatings;
            //List<MovieUserRating> userRatings;

            //var p = new DynamicParameters();
            //p.Add("MovieId", id);

            //using (SqlConnection sqlConnection = new SqlConnection(Settings.ConnectionString))
            //{
            //    Fullmovie = sqlConnection.Query<Movie>("SELECT MovieID, Title, ReleaseDate, MPAARatingID, DirectorName, Studio, UserRatingID, UserNotes, " +
            //                                        "actors, BorrowerName, DateBorrowed, DateReturned FROM movie WHERE MovieID = @MovieID", p).Single();

            //    userRatings = sqlConnection.Query<MovieUserRating>("select UserRatingID, UserRating, UserRatingDescription from UserRating").ToList();

            //    mpaaRatings = sqlConnection.Query<MovieMPAARating>("select MPAARatingID, MPAARating, MPAADescription from MPAARating").ToList();
            //}

            //IEnumerable<MovieUserRating> user = from rating in userRatings
            //                                        where rating.UserRatingID == Fullmovie.UserRatingId
            //                                        select rating;

            //// There must be an easier way to convert from an IEnumerable<MovieUserRating> to MovieUserRating but I couldn't figure it out
            //MovieUserRating[] user1 = user.ToArray();
            //MovieUserRating user2 = user1[0];

            //Fullmovie.MovieUserRating = user2;               

            //IEnumerable<MovieMPAARating> mpaa = from rating in mpaaRatings
            //                                where rating.MPAARatingID == Fullmovie.MPAARatingId
            //                                select rating;

            //// Same thing here, couldn't figure out how to do this conversion in an easier way
            //MovieMPAARating[] mpaa1 = mpaa.ToArray();
            //MovieMPAARating mpaa2 = mpaa1[0];

            //Fullmovie.MovieMPAARating = mpaa2;
                
            //return Fullmovie;
        }

        public List<Movie> GetAll(string searchString)
        {
            //List<Movie> movies;  ----------->GUYS I'M MOVING THIS OUTSIDE OF THE METHOD SO I CAN ACCESS THE LIST ELSEWHERE

            using (SqlConnection sqlConnection = new SqlConnection(
                Settings.ConnectionString))
            {
                movies = sqlConnection.Query<Movie>("SELECT MovieID, Title, MPAARatingID, UserRatingID, ReleaseDate, DirectorName, Studio, UserNotes, Actors, " + 
                                                    "BorrowerName, DateBorrowed, DateReturned FROM Movie").ToList();

                foreach (Movie movie in movies)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("MovieID", movie.MovieID);

                    movie.MovieUserRating = sqlConnection.Query<MovieUserRating>("SELECT Movie.UserRatingID, UserRating, UserRatingDescription FROM UserRating " +
                                                                        "INNER JOIN Movie ON UserRating.UserRatingID = Movie.UserRatingID " +
                                                                         "WHERE Movie.MovieID = @MovieID", parameters).Single();
                    movie.MovieMPAARating = sqlConnection.Query<MovieMPAARating>("SELECT Movie.MPAARatingID, MPAARating, MPAADescription FROM MPAARating " +
                                                                       "INNER JOIN Movie on MPAARating.MPAARatingID = Movie.MPAARatingID " +
                                                                       "WHERE Movie.MovieID = @MovieID", parameters).Single();        
                }
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                searchString.ToLower();

                movies = movies.Where(m => m.Title.ToLower().Contains(searchString)).ToList();
            }

            return movies;
        }


        public void Insert(Movie movie)
        {
            using (SqlConnection sqlConnection = new SqlConnection(Settings.ConnectionString))
            {
                string sqlQuery = "INSERT INTO Movie (Title, ReleaseDate, MPAARatingID, DirectorName, Studio, UserRatingID, " +
                                    "UserNotes, Actors, BorrowerName, DateBorrowed, DateReturned) " +
                                    "VALUES (@Title, @ReleaseDate, @MPAARatingID, @DirectorName, @Studio, @UserRatingID, " +
                                    "@UserNotes, @Actors, @BorrowerName, @DateBorrowed, @DateReturned)";
                try
                {
                    sqlConnection.Execute(sqlQuery, new
                    {
                        Title = movie.Title,
                        ReleaseDate = movie.ReleaseDate,
                        MPAARatingID = Int32.Parse(movie.MPAARatingId.ToString()),
                        DirectorName = movie.DirectorName,
                        Studio = movie.Studio,
                        UserRatingID = Int32.Parse(movie.UserRatingId.ToString()),
                        UserNotes = movie.UserNotes,
                        Actors = movie.Actors,
                        BorrowerName = movie.BorrowerName,
                        DateBorrowed = movie.DateBorrowed,
                        DateReturned = movie.DateReturned });
                }
                catch
                {

                }
            }
        }


        
    }
}