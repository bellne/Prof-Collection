using DVDLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DVDLibrary.Controllers
{
    public class HomeController : Controller
    {
        MovieRepository movieRepo = new MovieRepository();

        // GET: Home
        public ActionResult Index(string searchString)
        {
            var model = movieRepo.GetAll(searchString);

            return View(model);
        }
    }
}