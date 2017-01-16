using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MoviesTabTab.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        [Display(Name = "Number of Tickets")]        
        public byte NumberOfTickets { get; set; }

        [ForeignKey("Screening")]
        public int ScreeningId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual Screening Screening { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}