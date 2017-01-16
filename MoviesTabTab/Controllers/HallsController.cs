using MoviesTabTab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoviesTabTab.Controllers
{
    public class HallsController : Controller
    {
        public JsonResult GetHallSeatsNumber(int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var halls = db.Halls.ToList();

            var seats = from x in db.Halls where x.HallId == id select x.SeatsCapacity;

            return Json(seats, JsonRequestBehavior.AllowGet);
        }
    }
}