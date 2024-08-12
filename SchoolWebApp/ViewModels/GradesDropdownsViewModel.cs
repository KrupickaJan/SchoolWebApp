using SchoolWebApp.Models;

namespace SchoolWebApp.ViewModels;

public class GradesDropdownsViewModel
{
	public List<Student> Students { get; set; }
	public List<Subject> Subjects { get; set; }
	public GradesDropdownsViewModel()
	{
		Students = new();
		Subjects = new();
	}
}
