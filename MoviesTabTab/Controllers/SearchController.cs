using MoviesTabTab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoviesTabTab.Controllers
{
    public class SearchController : Controller
    {
        public JsonResult Search(string term)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var movies = (from x in db.Movies where x.Title.ToLower().Contains(term.ToLower()) select x).ToList();

            List<MovieHelper> moviesResults = new List<MovieHelper>();

            for (int i = 0; i < movies.Count; i++)
            {
                moviesResults.Add(new MovieHelper { Id = movies[i].MovieId.ToString(), Name = movies[i].Title });                
            }

            return Json(moviesResults, JsonRequestBehavior.AllowGet);
            
        }

        private class MovieHelper
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }
    }
}