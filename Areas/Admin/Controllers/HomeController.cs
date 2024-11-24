using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuickNotes.Areas.Admin.Models;
using QuickNotes.Models;

namespace QuickNotes.Areas.Admin.Controllers;

[Area("Admin")]
public class HomeController : Controller
{
    private readonly UserManager<AppUser> _userManager;

    public HomeController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult UserList()
    {
        List<AppUser> users = _userManager.Users.ToList();

        List<UserViewModel> userViewModels = users.Select(user => new UserViewModel()
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
        }).ToList();

        return View(userViewModels);
    }
}