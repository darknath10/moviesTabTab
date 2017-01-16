using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MoviesTabTab.Models;

namespace MoviesTabTab.Controllers
{
    [Authorize(Roles = "admin")]
    public class ScreeningsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Screenings
        public ActionResult Index()
        {
            var screenings = db.Screenings.Include(s => s.Hall).Include(s => s.Movie);
            return View(screenings.ToList());
        }

        // GET: Screenings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Screening screening = db.Screenings.Find(id);
            if (screening == null)
            {
                return HttpNotFound();
            }
            return View(screening);
        }

        // GET: Screenings/Create
        public ActionResult Create()
        {
            ViewBag.HallId = new SelectList(db.Halls, "HallId", "HallName");
            ViewBag.MovieId = new SelectList(db.Movies, "MovieId", "Title");
            return View();
        }

        // POST: Screenings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ScreeningId,ShowTime,AvailableSeats,MovieId,HallId")] Screening screening)
        {
            var screeningBefore = (from x in db.Screenings where (x.HallId == screening.HallId && x.ShowTime < screening.ShowTime) orderby x.ShowTime descending select x).FirstOrDefault();
            var screeningAfter = (from x in db.Screenings where (x.HallId == screening.HallId && x.ShowTime > screening.ShowTime) orderby x.ShowTime ascending select x).FirstOrDefault();
            var movie = db.Movies.Find(screening.MovieId);
            var scenario = Cases(screeningBefore, screeningAfter);

            switch (scenario)
            {
                case 1:
                    if (ModelState.IsValid)
                    {
                        db.Screenings.Add(screening);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    break;
                case 2:
                    if (screening.ShowTime.AddMinutes((double)movie.Runtime + 15) < screeningAfter.ShowTime)
                    {
                        goto case 1;
                    }                    
                    break;
                case 3:
                    if (screeningBefore.ShowTime.AddMinutes((double)screeningBefore.Movie.Runtime + 15) < screening.ShowTime)
                    {
                        goto case 1;
                    }
                    break;
                case 4:
                    if (screeningBefore.ShowTime.AddMinutes((double)screeningBefore.Movie.Runtime + 15) < screening.ShowTime && screening.ShowTime.AddMinutes((double)movie.Runtime + 15) < screeningAfter.ShowTime)
                    {
                        goto case 1;
                    }
                    break;
                default:                    
                    break;
            }

            ModelState.AddModelError("ShowTime", "The hall is not available at this time. Check the schedules and choose an other day/hour.");
            ViewBag.HallId = new SelectList(db.Halls, "HallId", "HallName", screening.HallId);
            ViewBag.MovieId = new SelectList(db.Movies, "MovieId", "Title", screening.MovieId);
            return View(screening);
        }

        // GET: Screenings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Screening screening = db.Screenings.Find(id);
            if (screening == null)
            {
                return HttpNotFound();
            }
            ViewBag.HallId = new SelectList(db.Halls, "HallId", "HallName", screening.HallId);
            ViewBag.MovieId = new SelectList(db.Movies, "MovieId", "Title", screening.MovieId);
            return View(screening);
        }

        // POST: Screenings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ScreeningId,ShowTime,AvailableSeats,MovieId,HallId")] Screening screening)
        {
            var screeningBefore = (from x in db.Screenings where (x.HallId == screening.HallId && x.ShowTime < screening.ShowTime) orderby x.ShowTime descending select x).FirstOrDefault();
            var screeningAfter = (from x in db.Screenings where (x.HallId == screening.HallId && x.ShowTime > screening.ShowTime) orderby x.ShowTime ascending select x).FirstOrDefault();
            var movie = db.Movies.Find(screening.MovieId);
            var scenario = Cases(screeningBefore, screeningAfter);

            switch (scenario)
            {
                case 1:
                    if (ModelState.IsValid)
                    {
                        db.Entry(screening).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    break;
                case 2:
                    if (screening.ShowTime.AddMinutes((double)movie.Runtime + 15) < screeningAfter.ShowTime)
                    {
                        goto case 1;
                    }
                    break;
                case 3:
                    if (screeningBefore.ShowTime.AddMinutes((double)screeningBefore.Movie.Runtime + 15) < screening.ShowTime)
                    {
                        goto case 1;
                    }
                    break;
                case 4:
                    if (screeningBefore.ShowTime.AddMinutes((double)screeningBefore.Movie.Runtime + 15) < screening.ShowTime && screening.ShowTime.AddMinutes((double)movie.Runtime + 15) < screeningAfter.ShowTime)
                    {
                        goto case 1;
                    }
                    break;
                default:
                    break;
            }

            ModelState.AddModelError("ShowTime", "The hall is not available at this time. Check the schedules and choose an other day/hour.");
            ViewBag.HallId = new SelectList(db.Halls, "HallId", "HallName", screening.HallId);
            ViewBag.MovieId = new SelectList(db.Movies, "MovieId", "Title", screening.MovieId);
            return View(screening);
        }

        // GET: Screenings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Screening screening = db.Screenings.Find(id);
            if (screening == null)
            {
                return HttpNotFound();
            }
            return View(screening);
        }

        // POST: Screenings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Screening screening = db.Screenings.Find(id);
            db.Screenings.Remove(screening);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public static int Cases(Screening scr1, Screening scr2)
        {
            if (scr1 == null && scr2 == null)
            {
                return 1;
            }
            if (scr1 == null && scr2 != null)
            {
                return 2;
            }
            if (scr1 != null && scr2 == null)
            {
                return 3;
            }
            else
            {
                return 4;
            }
        }
    }
}
