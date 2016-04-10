using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVDLibrary.Models
{
    public interface IMovieRepository
    {
        List<Movie> GetAll(string search);
        Movie Get(int MovieId);
        void Insert(Movie movie);
        void Edit(Movie movie);
        void Delete(int id);

    }
}
