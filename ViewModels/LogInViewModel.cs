using System.ComponentModel.DataAnnotations;

namespace QuickNotes.ViewModels;

public class LogInViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
    
    public bool RememberMe { get; set; }
}