using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace WeddingPlanner.Models
{
    public class Wedding
    {
        [Key]
        public int WeddingId { get; set; }
        // %%%%%%%%%%%%%%%%%%%%%%%Wedder one Name%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        [Display(Name = "Wedder One")]
        [Required(ErrorMessage = "Enter name")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name must be letters only")]
        public string WedderOne { get; set; }
        // %%%%%%%%%%%%%%%%%%%%%%%Wedder Two Name%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        [Display(Name = "Wedder Two")]
        [Required(ErrorMessage = "Enter Name")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last name must be letters only")]
        public string WedderTwo { get; set; }
        // %%%%%%%%%%%%%%%%%%%%%%%Wedding Date%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

        [DataType(DataType.Date)]
        [Required]
        public DateTime Date { get; set; }
        // %%%%%%%%%%%%%%%%%%%%%%%Wedding Address%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        [Display(Name = "Wedding Address")]
        [Required(ErrorMessage= "Please Include A Address")]
        [MinLength(6, ErrorMessage="Please include a valide address longer then 2 charecters")]
        public string Address{ get; set;}
        // %%%%%%%%%%%%%%%%%%%%%%%Update time and date%%%%%%%%%%%%%
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        // %%%%%%%%%%%%%%%%%%%%%%%Gets the UserId %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        public int UserId { get; set; }
        // %%%%%%%%%%%%%%%%%%%%%%%Who posted the Wedding  %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        public User PostedBy { get; set;}
        // %%%%%%%%%%%%%%%%%%%%%%%Gets you to the middle table %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        public List<Guest> Guests  { get; set;}
    }
}