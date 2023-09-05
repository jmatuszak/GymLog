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
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[!@#$%^&*(),.?"":{}|<>])(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d!@#$%^&*(),.?"":{}|<>]{10,}$",
        ErrorMessage = "Password must contain at least 10 characters, 1 uppercase letter, 1 lowercase letter, 1 number and 1 special character.")]
       
        public string Password { get; set; }
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password do not match")]
        [Required(ErrorMessage = "Confirm Password is required")]
        public string ConfirmPassword { get; set; }
    }
}
