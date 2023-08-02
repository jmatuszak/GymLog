using System.ComponentModel.DataAnnotations;

namespace GymLog.ViewModels
{
    public class RegisterVM
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Email address is required")]
        public string EmailAddress { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password do not match")]
        [Required(ErrorMessage = "Email address is required")]
        public string ConfirmPassword { get; set; }
    }
}
