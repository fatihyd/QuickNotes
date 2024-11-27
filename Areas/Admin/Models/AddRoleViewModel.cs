using System.ComponentModel.DataAnnotations;

namespace QuickNotes.Areas.Admin.Models;

public class AddRoleViewModel
{
    [Required(ErrorMessage = "Please enter a name")]
    public string RoleName { get; set; }
}