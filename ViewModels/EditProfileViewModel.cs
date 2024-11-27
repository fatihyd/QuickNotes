using System.ComponentModel.DataAnnotations;

namespace QuickNotes.ViewModels;

public class EditProfileViewModel
{
    public string PhoneNumber { get; set; }
    public string Bio { get; set; }
}