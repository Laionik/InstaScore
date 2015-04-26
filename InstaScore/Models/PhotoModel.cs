using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace InstaScore.Models
{
    public class PhotosContext : DbContext
    {
        public PhotosContext()
            : base("DefaultConnection")
        {
        }
        public DbSet<photos> dbphoto { get; set; }
    }
    public class photos
    {
        [Key]
        public int photoID { get; set; }
        public string photoURL { get; set; }
        public int photoScore { get; set; }
        public int photoTotal { get; set; }
        public bool photoVisible { get; set; }
    }
}