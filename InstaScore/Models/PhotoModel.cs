using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace InstaScore.Models
{
    public class MovieDBContext : DbContext
    {
        public DbSet<PhotoModel> Photos { get; set; }
    }

    public class PhotoModel
    {
        public int ID { get; set; }
        public string link { get; set; }
        public int score { get; set; }
        public int total { get; set; }
    }
}