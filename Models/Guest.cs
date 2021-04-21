using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace WeddingPlanner.Models
{
    public class Guest
    {
        [Key]
        public int GuestId { get; set; }
        // %%%%%%%%%%%%%%%%%%%%%%%Connect to one User%%%%%%%%%%%%%&&&&&&&&&&&&&&&&&&&&&
        public int UserId { get; set; }
        // %%%%%%%%%%%%%%%%%%%%%%%Connects to all Users%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        public User UserToWedding { get; set; }

        // %%%%%%%%%%%%%%%%%%%%%%%Connect to  one Wedding %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        public int WeddingId { get; set; }

        // %%%%%%%%%%%%%%%%%%%%%%%Connect to all Weddings %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        public Wedding WeddingToUser { get; set; }

        // %%%%%%%%%%%%%%%%%%%%%%%Update time and date%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}