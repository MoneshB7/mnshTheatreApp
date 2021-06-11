using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace mnshTheatreApp.Models
{
    public class movieModel
    {   [Key]
        public string MovieId { get; set; } 
        public string MovieName { get; set; }
        public string Language { get; set; }
        public string AverageRating { get; set; }
        public string Description { get; set; }
        public string PosterURL { get; set; }
        public string Location { get; set; }

        [Required]
        public string Name { get; set; }
        [DisplayName("No of Seats")]
        [Range(1,10,ErrorMessage =" Only 10 seats can be booked.")]
        public int SeatNo { get; set; }
        [Required]
        public string EmailID { get; set; }
        public string QRCodeImage { get; set; }

        public List<movieModel> indexList { get; set; }

        public class Credential
        {
            public string Email { get; set; }
            public string Password { get; set; }

            public string QRCodePath { get; set; }

            public string LogPath { get; set; }
        }

    }
}
