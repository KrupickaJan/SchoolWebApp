using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolWebApp.DTO;
using SchoolWebApp.Services;
using System.Dynamic;

namespace SchoolWebApp.Controllers;
[Authorize]
public class SubjectsController : Controller
{
    public SubjectService _service;

    public SubjectsController(SubjectService service)
    {
        _service = service;

	}

    public IActionResult Index()
    {
		
		var allsubjects = _service.GetSubjects();
        return View(allsubjects);
    }
	[Authorize(Roles = "Teacher, Admin")]
	public IActionResult Create()
    {
		return View();
	}
	[Authorize(Roles = "Teacher, Admin")]
	[HttpPost]
    public async Task<IActionResult> Create(SubjectDTO subject)
    {
        if (ModelState.IsValid)
        {
			await _service.AddSubjectAsync(subject);
			return RedirectToAction("Index");
		}
        else
        {
			return View();
		}
    }
	[Authorize(Roles = "Teacher, Admin")]
	public async Task<IActionResult> Edit(int id)
    {
        var subjectForEdit = await _service.GetSubjectByIdAsync(id);
        if (subjectForEdit == null)
        {
            return View("NotFound");
        }
        return View(subjectForEdit);
    }
	[Authorize(Roles = "Teacher, Admin")]
	[HttpPost]
    public async Task<IActionResult> Edit(SubjectDTO subject)
    {
        await _service.EditSudentAsync(subject);
        return RedirectToAction("Index");
    }
	[Authorize(Roles = "Teacher, Admin")]
	[HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.RemoveSubjectAsync(id);
        return RedirectToAction("Index");
    }
}
