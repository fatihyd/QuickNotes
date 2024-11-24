using System.ComponentModel.DataAnnotations;

namespace QuickNotes.ViewModels;

public class SignUpViewModel
{
    [Required(ErrorMessage = "Username is required")]
    public string UserName { get; set; }
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Phone number is required")]
    public string PhoneNumber { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
    
    [Required(ErrorMessage = "Password confirmation is required")]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
    public string PasswordConfirm { get; set; }
}