
using System.ComponentModel.DataAnnotations;

namespace QuickNotes.ViewModels;

public class ChangePasswordViewModel
{
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Please enter the old password")]
    public string OldPassword { get; set; }
    
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Please enter the new password")]
    public string NewPassword { get; set; }
    
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Please confirm the new password")]
    [Compare(nameof(NewPassword), ErrorMessage = "The new password and confirmation password do not match.")]
    public string ConfirmNewPassword { get; set; }

}