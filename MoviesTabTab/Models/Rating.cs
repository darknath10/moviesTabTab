using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MoviesTabTab.Models
{
    public class Rating
    {
        [Key]
        public int RatingId { get; set; }

        [Range(0.0d, 10.0d)]
        public double Rate { get; set; }

        [ForeignKey("Movie")]
        public int MovieId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual Movie Movie { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}