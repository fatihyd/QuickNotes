using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuickNotes.Models;

namespace QuickNotes.Contexts;

public class QuickNotesDbContext : IdentityDbContext<AppUser, AppRole, string>
{
    public QuickNotesDbContext(DbContextOptions options) : base(options)
    {
    }
    
}