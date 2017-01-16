using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MoviesTabTab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoviesTabTab.Controllers
{
    [Authorize]
    public class RatingController : Controller
    {
        ApplicationDbContext db;
        UserManager<ApplicationUser> manager;

        public RatingController()
        {
            db = new ApplicationDbContext();
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        //GET
        public PartialViewResult Rating(int movieId)
        {
            var movie = (from x in db.Movies where x.MovieId == movieId select x).First();
            var userId = User.Identity.GetUserId();
            var rated = (from x in db.Ratings where x.MovieId == movieId && x.UserId == userId select x).FirstOrDefault();

            if (rated != null)
            {
                var result = rated.Rate;
                return PartialView("_MovieRated", rated);                
            }
            else
            {
                return PartialView("_MovieRating", movie);
            }
        }

        public ActionResult NewRating(int movieId, double rate)
        {
            var userId = User.Identity.GetUserId();
            Rating newRate = new Rating { UserId = userId, MovieId = movieId, Rate = rate };
            db.Ratings.Add(newRate);
            db.SaveChanges();
            return PartialView("_MovieRated", newRate);
        }
    }
}