using SchoolWebApp.Models;

namespace SchoolWebApp.ViewModels;


public class SubjectsViewModel
{
	public Subject Subject { get; set; }
	public IEnumerable<Subject> Subjects { get; set; }
}
