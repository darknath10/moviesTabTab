using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MoviesTabTab.Models
{
    public class Screening
    {
        [Key]
        public int ScreeningId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]        
        [Display(Name = "Show Time")]
        public DateTime ShowTime { get; set; }

        [Display(Name = "Available Seats")]
        public byte? AvailableSeats { get; set; }

        [ForeignKey("Movie")]
        public int MovieId { get; set; }

        [ForeignKey("Hall")]
        public int HallId { get; set; }

        public virtual Movie Movie { get; set; }

        public virtual Hall Hall { get; set; }

        public ICollection<Booking> Bookings { get; set; }

        public Screening()
        {
            this.Bookings = new HashSet<Booking>();
        }
    }
}