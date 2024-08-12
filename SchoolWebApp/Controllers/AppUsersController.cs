using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolWebApp.Models;
using SchoolWebApp.ViewModels;
namespace SchoolWebApp.Controllers;
[Authorize(Roles = "Teacher, Admin")]
public class AppUsersController:Controller
{
	private UserManager<AppUser> _userManager;
	private IPasswordHasher<AppUser> _passwordHasher;
	private IPasswordValidator<AppUser> _passwordValidator;

	public AppUsersController(UserManager<AppUser> userManager, IPasswordHasher<AppUser> passwordHasher, IPasswordValidator<AppUser> passwordValidator)
	{
		_userManager = userManager;
		_passwordHasher = passwordHasher;
		_passwordValidator = passwordValidator;
	}
	public IActionResult Index()
	{
		return View(_userManager.Users);
	}

	public IActionResult Create()
	{
		return View();
	}
	[HttpPost]
	public async Task<IActionResult> Create(AppUserViewModel appUserViewModel)
	{
		if (ModelState.IsValid) 
		{
			AppUser newUser = new AppUser()
			{
				UserName = appUserViewModel.UserName,
				Email = appUserViewModel.Email,
			};
			IdentityResult result = await _userManager.CreateAsync(newUser, appUserViewModel.Password);

			if (result.Succeeded)
			{
				return RedirectToAction("Index");
			}
			else
			{
				foreach(var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
			}
		}
		return View(appUserViewModel);
	}

	public async Task<IActionResult> Edit(string id)
	{
		AppUser? userToEdit = await _userManager.FindByIdAsync(id);

		if(userToEdit == null)
		{
			return View("NotFound");
		}
		else
		{
			return View(userToEdit);
		}
	}
	[HttpPost]
	public async Task<IActionResult> Edit(string id, string email, string password)
	{
		AppUser? userToEdit = await _userManager.FindByIdAsync(id);
		if(userToEdit != null)
		{
			IdentityResult validPassword;

			if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(password))
			{
				userToEdit.Email = email;
				validPassword = await _passwordValidator.ValidateAsync(_userManager, userToEdit, password);
				if (validPassword.Succeeded)
				{
					userToEdit.PasswordHash = _passwordHasher.HashPassword(userToEdit, password);
					IdentityResult identityResult = await _userManager.UpdateAsync(userToEdit);
					if (identityResult.Succeeded)
					{
						return RedirectToAction("Index");
					}
					else
					{
						AddErrors(identityResult);
					}
				}
				else
				{
					AddErrors(validPassword);
				}
			}
		}
		else
		{
			ModelState.AddModelError("", "User not found");
		}
		return View(userToEdit);
	}

	private void AddErrors(IdentityResult identityResult)
	{
		foreach (var error in identityResult.Errors)
		{
			ModelState.AddModelError("", error.Description);
		}
	}

	[HttpPost]
	public async Task<IActionResult> Delete(string id)
	{
		AppUser? appUserToDelete = await _userManager.FindByIdAsync(id);
		if(appUserToDelete != null) {
			var result = await _userManager.DeleteAsync(appUserToDelete);
			if (result.Succeeded)
			{
				return RedirectToAction("Index");
			}
			else
			{
				AddErrors(result);
			}
		}
		else
		{
			ModelState.AddModelError("", "User not found");
		}
		return View("Index", _userManager.Users);
	}
}
