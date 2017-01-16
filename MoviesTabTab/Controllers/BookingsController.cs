using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MoviesTabTab.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MoviesTabTab.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        ApplicationDbContext db;

        UserManager<ApplicationUser> manager;

        public BookingsController()
        {
            db = new ApplicationDbContext();
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        //GET
        public ActionResult MyBookings()
        {
            var currentUserId = User.Identity.GetUserId();
            var bookings = (from x in db.Bookings where x.UserId == currentUserId orderby x.Screening.ShowTime descending select x).ToList();
            return View(bookings);
        }

        //GET
        [HandleError(View = "Error")]
        public ActionResult Booking(int? movieId)
        {
            var movie = (from x in db.Movies where x.MovieId == movieId select x).First();
            var screenings = (from x in db.Screenings where (x.MovieId == movieId && x.ShowTime > DateTime.Now) select x).ToList();
            if (screenings.Count == 0)
            {
                return View("NoScreening");
            }
            ViewBag.MovieTitle = movie.Title;
            ViewBag.PosterUrl = movie.PosterUrl;
            ViewBag.ScreeningId = new SelectList(screenings, "ScreeningId", "ShowTime");
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]        
        public async Task<ActionResult> Booking([Bind(Include = "UserId,ScreeningId,NumberOfTickets")] Booking booking)
        {
            var screening = (from x in db.Screenings where x.ScreeningId == booking.ScreeningId select x).First();
            var movie = (from x in db.Movies where x.MovieId == screening.MovieId select x).First();
            string errorMessage;
            switch (booking.NumberOfTickets > 0)
            {
                case false:
                    errorMessage = "You must choose at least 1 ticket in order to book this show.";
                    ModelState.AddModelError("NumberOfTickets", errorMessage);
                    break;
                case true:
                    if (booking.NumberOfTickets > screening.AvailableSeats)
                    {
                        errorMessage = string.Format("There are only {0} seats left for this show.", screening.AvailableSeats);
                        ModelState.AddModelError("NumberOfTickets", errorMessage);
                    }
                    else
                    {
                        var currentUser = manager.FindById(User.Identity.GetUserId());

                        if (ModelState.IsValid)
                        {
                            booking.User = currentUser;
                            db.Bookings.Add(booking);

                            screening.AvailableSeats -= booking.NumberOfTickets;

                            db.SaveChanges();

                            await SendMessageAsync(currentUser.Email, booking);

                            return View("BookingConfirmation", booking);
                        }
                    }
                    break;
            }

            ViewBag.ScreeningId = new SelectList(db.Screenings, "ScreeningId", "ShowTime");
            ViewBag.MovieTitle = movie.Title;
            ViewBag.PosterUrl = movie.PosterUrl;
            return View(booking);
        }

        public async Task SendMessageAsync(string customerEmail, Booking booking)
        {
            string from = ConfigurationManager.AppSettings["fromEmail"];
            string to = customerEmail;
            string subject = "Your reservation is confirmed";
            string htmlBody = "<h2>" + booking.Screening.Movie.Title + " - Movies-Tab/Tab Theaters</h2>" +
            "<h4>" + booking.Screening.ShowTime.ToShortDateString() + "-" + booking.Screening.ShowTime.ToShortTimeString() + "</h4>" +
            "<h4>Hall: " + booking.Screening.Hall.HallName + "</h4>" +
            "<h4>" + booking.NumberOfTickets + "seat(s)</h4>";

            MailMessage mailMessage = new MailMessage(from, to, subject, htmlBody) { IsBodyHtml = true };
            SmtpClient client = new SmtpClient();

            await client.SendMailAsync(mailMessage);
        }
    }
}