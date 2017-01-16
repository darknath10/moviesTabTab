using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MoviesTabTab.Models
{
    public class Hall
    {
        [Key]
        public int HallId { get; set; }

        [Required]
        [Display(Name = "Hall Name")]
        public string HallName { get; set; }

        [Display(Name = "Capacity")]
        public byte? SeatsCapacity { get; set; }

        public virtual ICollection<Screening> Screenings { get; set; }

        public Hall()
        {
            Screenings = new HashSet<Screening>();
        }
    }
}