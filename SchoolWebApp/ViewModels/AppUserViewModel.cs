using System.ComponentModel.DataAnnotations;

namespace SchoolWebApp.ViewModels;

public class AppUserViewModel
{
	public string UserName { get; set; }
	public string Password { get; set; }
	[EmailAddress(ErrorMessage ="Invalid email address.")]
	public string Email { get; set; }
}
