using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using QuickNotes.Areas.Admin.Models;
using QuickNotes.Models;

namespace QuickNotes.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class RolesController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;

    public RolesController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IActionResult> Index()
    {
        List<RoleViewModel> roleViewModels = await _roleManager.Roles.Select(role => new RoleViewModel()
        {
            Id = role.Id,
            Name = role.Name,
        }).ToListAsync();

        return View(roleViewModels);
    }
    
    public IActionResult AddRole()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> AddRole(AddRoleViewModel model)
    {
        IdentityResult identityResult = await _roleManager.CreateAsync(new AppRole() { Name = model.RoleName });

        if (!identityResult.Succeeded)
        {
            foreach (var error in identityResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
                return View();
            }
        }
        
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> AssignRoles(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        
        var roles = await _roleManager.Roles.ToListAsync();
        
        var assignRolesViewModels = new List<AssignRolesViewModel>();

        var userRoles = await _userManager.GetRolesAsync(user);

        foreach (var role in roles)
        {
            var assignRolesViewModel = new AssignRolesViewModel()
            {
                Id = role.Id,
                Name = role.Name
            };

            if (userRoles.Contains(role.Name))
            {
                assignRolesViewModel.Exists = true;
            }
            
            assignRolesViewModels.Add(assignRolesViewModel);
        }

        return View(assignRolesViewModels);
    }
}
