using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuickNotes.Models;
using QuickNotes.ViewModels;

namespace QuickNotes.Controllers;

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

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult SignUp()
    {
        return View();
    }

[HttpPost]
public async Task<IActionResult> SignUp(SignUpViewModel model)
{
    // Check if the model's data is valid
    // according to validation rules defined in the SignUpViewModel
    if (!ModelState.IsValid)
    {
        // If validation fails, return the SignUp view and re-display the form
        return View();
    }

    // Create a new AppUser instance from the submitted data
    var user = new AppUser()
    {
        UserName = model.Email,
        Email = model.Email,
        PhoneNumber = model.PhoneNumber
    };

    // Use the UserManager to create the user
    // The password will be hashed and stored securely in the database
    var identityResult = await _userManager.CreateAsync(user, model.Password);

    // Check if the user creation was successful
    if (identityResult.Succeeded)
    {
        // If successful, store a success message in TempData
        TempData["SuccessMessage"] = "Signed up successfully!";

        // Redirect to the GET version of the SignUp action
        // This follows the Post/Redirect/Get pattern to prevent duplicate submissions
        return RedirectToAction(nameof(HomeController.SignUp));
    }

    // If user creation fails:
    
    // Iterate through the errors provided by Identity
    foreach (var error in identityResult.Errors)
    {
        // Add each error to the ModelState
        ModelState.AddModelError(string.Empty, error.Description);
    }

    // Re-display the SignUp view with the current ModelState and errors.
    return View();
}


    public IActionResult LogIn()
    {
        return View();
    }

    public IActionResult LogOut()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
