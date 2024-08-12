using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolWebApp.DTO;
using SchoolWebApp.Services;

namespace SchoolWebApp.Controllers;
[Authorize]
public class StudentsController : Controller
{
    public StudentService _service;

    public StudentsController(StudentService service)
    {
        _service = service;
    }

    public IActionResult Index()
    {
		var allStudents = _service.GetStudents();
        return View(allStudents);
    }
    [Authorize(Roles ="Teacher, Admin")]
    public IActionResult Create()
    {
        return View();
    }
	[Authorize(Roles = "Teacher, Admin")]
	[HttpPost]
    public async Task<IActionResult> Create(StudentDTO student)
    {
        if (ModelState.IsValid)
        {
            await _service.AddStudentAsync(student);
			return RedirectToAction();
		}
        return View();
    }
	[Authorize(Roles = "Teacher, Admin")]
	public async Task<IActionResult> Edit(int id)
    {
        var studentForEdit = await _service.GetStudentByIdAsync(id);
        if (studentForEdit == null)
        {
            return View("NotFound");
        }
        return View(studentForEdit);
    }
	[Authorize(Roles = "Teacher, Admin")]
	[HttpPost]
    public async Task<IActionResult> Edit(StudentDTO student)
    {
        await _service.EditSudentAsync(student);
        return RedirectToAction("Index");
    }
	[Authorize(Roles = "Teacher, Admin")]
	[HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.RemoveStudentAsync(id);
        return RedirectToAction("Index");
    }
}
