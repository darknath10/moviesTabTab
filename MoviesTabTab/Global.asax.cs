using MoviesTabTab.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MoviesTabTab
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static DateTime lastUpdate;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            lastUpdate = DateTime.Now; // to test the function set: lastUpdate = DateTime.Now.AddDays(-2);
        }

        void Session_Start(object sender, EventArgs e)
        {
            UpdateMoviesStats();
        }

        static void UpdateMoviesStats()
        {
            var oneDayAgo = DateTime.Now.AddDays(-1);

            if (lastUpdate < oneDayAgo)
            {
                ApplicationDbContext db = new ApplicationDbContext();
                var movies = db.Movies.ToList();

                if (movies !=null)
                {
                    foreach (var item in movies)
                    {
                        if (item.TmdbMovieId != null)
                        {
                            var id = item.TmdbMovieId;
                            var adress = string.Format("http://api.themoviedb.org/3/movie/{0}?api_key=d1dd5a7fd77c933e088112709eb711e7", id);
                            var request = System.Net.WebRequest.Create(adress) as System.Net.HttpWebRequest;
                            request.Method = "GET";
                            request.Accept = "application / json";
                            request.ContentLength = 0;
                            string responseContent;
                            using (var response = request.GetResponse() as System.Net.HttpWebResponse)
                            {
                                using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
                                {
                                    responseContent = reader.ReadToEnd();
                                }
                            }

                            MovieModelForUpdate m = JsonConvert.DeserializeObject<MovieModelForUpdate>(responseContent);
                            item.Score = m.vote_average;
                            item.VoteCount = m.vote_count;
                            db.SaveChanges();
                        }                        
                    }                    
                }
                lastUpdate = DateTime.Now;
            }
        }

        private class MovieModelForUpdate
        {
            public double vote_average { get; set; }
            public int vote_count { get; set; }
            public double popularity { get; set; }
        }
    }
}
