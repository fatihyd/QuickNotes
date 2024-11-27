using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuickNotes.Models;
using QuickNotes.ViewModels;

namespace QuickNotes.Controllers;

public class HomeController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
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
            UserName = model.UserName,
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

    [HttpPost]
    public async Task<IActionResult> LogIn(LogInViewModel model)
    {
        // Try to find a user in the database by the provided email
        var user = await _userManager.FindByEmailAsync(model.Email);

        // Check if the user exists
        if (user == null)
        {
            // If no user is found, add a general error message to the ModelState
            ModelState.AddModelError(string.Empty, "Invalid username or password.");

            // Return the LogIn view, keeping any errors and the form input data intact
            return View();
        }

        // Try to sign the user in
        // The RememberMe parameter indicates whether the user wants to stay signed in across sessions
        // The fourth parameter (false) disables account lockout for failed login attempts
        var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

        // Check if the sign-in attempt was successful
        if (signInResult.Succeeded)
        {
            // If the credentials are correct, redirect the user to the home page
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        // If the sign-in attempt fails, add an error to the ModelState
        ModelState.AddModelError(string.Empty, "Invalid sign-in attempt.");

        // Re-display the Login view with the current ModelState and error messages
        return View();
    }
    
    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    [Authorize]
    public async Task<IActionResult> About()
    {
        var user = await _userManager.FindByNameAsync(User.Identity.Name);

        var aboutUserViewModel = new AboutUserViewModel()
        {
            UserName = user.UserName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };
        
        return View(aboutUserViewModel);
    }

    [Authorize]
    public async Task<IActionResult> EditProfile()
    {
        var user = await _userManager.FindByNameAsync(User.Identity.Name);
        var editProfileViewModel = new EditProfileViewModel()
        {
            PhoneNumber = user.PhoneNumber
        };
        return View(editProfileViewModel);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> EditProfile(EditProfileViewModel model)
    {
        var user = await _userManager.FindByNameAsync(User.Identity.Name);

        if (user.PhoneNumber != model.PhoneNumber)
        {
            user.PhoneNumber = model.PhoneNumber;
        }

        // Update the user
        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            // Refresh the sign-in to update the authentication cookie
            await _signInManager.RefreshSignInAsync(user);
            return RedirectToAction(nameof(About));
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
        return View(model);
    }

    [Authorize]
    public IActionResult ChangePassword()
    {
        return View();
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.FindByNameAsync(User.Identity.Name);
        if (user == null)
        {
            return RedirectToAction("LogIn", "Home");
        }

        // Verify the old password
        var checkPassword = await _userManager.CheckPasswordAsync(user, model.OldPassword);
        if (!checkPassword)
        {
            ModelState.AddModelError(nameof(model.OldPassword), "Old password is incorrect.");
            return View(model);
        }

        // Check if the new password is the same as the old password
        if (model.OldPassword == model.NewPassword)
        {
            ModelState.AddModelError(nameof(model.NewPassword), "The new password cannot be the same as the old password.");
            return View(model);
        }

        // Change the password
        var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        // Refresh the sign-in to update the authentication cookie
        await _signInManager.RefreshSignInAsync(user);

        TempData["SuccessMessage"] = "Password changed successfully!";
        return RedirectToAction(nameof(ChangePassword));
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
