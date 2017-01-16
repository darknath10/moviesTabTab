using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MoviesTabTab.Models
{
    public class Movie
    {
        [Key]
        public int MovieId { get; set; }

        public string TmdbMovieId { get; set; }

        [Required]
        public string Title { get; set; }

        [Display(Name = "Original Title")]
        public string OriginalTitle { get; set; }

        public short? Runtime { get; set; }

        [DataType(DataType.Date)]        
        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }

        public string Tagline { get; set; }

        public string Synopsis { get; set; }

        public string Director { get; set; }

        [Range(0.0d, 10.0d)]
        public double? Score { get; set; }

        [Display(Name = "Vote Count")]
        public int? VoteCount { get; set; }

        public double? Popularity { get; set; }

        public string PosterUrl { get; set; }

        public string BackdropPath { get; set; }

        public string TrailerUrl { get; set; }

        public virtual ICollection<Screening> Screenings { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; }

        public Movie()
        {
            this.Screenings = new HashSet<Screening>();
            this.Ratings = new HashSet<Rating>();
        }
    }
}