using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using mnshTheatreApp.Models;

namespace mnshTheatreApp.Data
{
    public class mnshTheatreAppContext : DbContext
    {
        public mnshTheatreAppContext (DbContextOptions<mnshTheatreAppContext> options)
            : base(options)
        {
        }

        public DbSet<mnshTheatreApp.Models.movieModel> movieModel { get; set; }
    }
}
