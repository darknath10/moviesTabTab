using MoviesTabTab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoviesTabTab.Controllers
{
    [RequireHttps]
    [AllowAnonymous]
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var movies = db.Movies.ToList();

            if (movies.Count < 3)
            {
                return View("NowPlaying", movies);
            }
            else
            {
                var threePopularMovies = (from x in movies orderby x.Popularity descending select x).Take(3).ToList();
                return View(threePopularMovies);
            }            
        }

        public ActionResult NowPlaying()
        {
            var movies = (from x in db.Movies select x).ToList();
            return View(movies);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}