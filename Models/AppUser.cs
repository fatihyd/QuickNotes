using Microsoft.AspNetCore.Identity;

namespace QuickNotes.Models;

public class AppUser : IdentityUser
{
    public string Bio { get; set; }
    public string Picture { get; set; }
}