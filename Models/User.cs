using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
namespace WeddingPlanner.Models{
public class User
{
    [Key]
    public int UserId {get;set;}
    // %%%%%%%%%%%%%%%%%%%%%%%First Name%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
    [Display(Name = "First Name")]
    [Required(ErrorMessage = "Enter your First name")]
    [MinLength(2, ErrorMessage = "First name must be at least 2 characters")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name must be letters only")]
    public string FirstName {get;set;}
    // %%%%%%%%%%%%%%%%%%%%%%%Last Name%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
    [Display(Name = "Last Name")]
    [Required(ErrorMessage = "Enter your last name")]
    [MinLength(2, ErrorMessage = "Last name must be at least 2 characters")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last name must be letters only")]
    public string LastName {get;set;}
    // %%%%%%%%%%%%%%%%%%%%%%%Email%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
    [Display(Name = "Email Address")]
    [EmailAddress]
    [Required]
    public string Email {get;set;}
    // %%%%%%%%%%%%%%%%%%%%%%%Password%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
    [Required]
    [MinLength(8, ErrorMessage = "Password Must be at least 8 characters")]
    [DataType(DataType.Password)]
    public string Password {get;set;}

    // %%%%%%%%%%%%%%%%%%%%%%%List of Weddings %%%%%%%%%%%%%
    public List<Guest> Guests {get;set;}
    // %%%%%%%%%%%%%%%%%%%%%%%List of wedding you user posted  %%%%%%%%%%%%%
    public List<Wedding> MyPost {get;set;}

    // %%%%%%%%%%%%%%%%%%%%%%%Update time and date%%%%%%%%%%%%%
    public DateTime CreatedAt {get;set;} = DateTime.Now;
    public DateTime UpdatedAt {get;set;} = DateTime.Now;
    // %%%%%%%%%%%%%%%%%%%%%%%%%Will not be Mapped but will Confirm password%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
    [NotMapped]
    [Compare("Password", ErrorMessage = "Please ensure that the password confirmation matches the password")]
    [DataType(DataType.Password)]
    public string Confirm {get;set;}
}
}