using DVDLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DVDLibrary.Controllers
{
    public class DVDController : Controller
    {
        MovieRepository movieRepo = new MovieRepository();

        // GET: 
        public ActionResult Delete(int id)
        {
            movieRepo.Delete(id);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Get (int id)
        {
            var model = movieRepo.Get(id);

            return View(model);
        }

        public ActionResult Add()
        {
            var model = new AddMovieVM();
            model.Movie = new Movie();

            return View(model);
        }

        [HttpPost]
        public ActionResult PostMovie(AddMovieVM newMovie)
        {
            movieRepo.Insert(newMovie.Movie);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Edit(int id)
        {
            var model = new AddMovieVM();
            model.Movie = movieRepo.Get(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Movie movie)
        {
            movieRepo.Edit(movie);

            return RedirectToAction("Index", "Home");
        }
    }
}