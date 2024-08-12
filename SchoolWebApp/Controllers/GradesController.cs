using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolWebApp.DTO;
using SchoolWebApp.Services;

namespace SchoolWebApp.Controllers;
[Authorize]
public class GradesController : Controller
{
    private GradeService _gradeService;

    public GradesController(GradeService gradeService)
    {
        _gradeService = gradeService;
    }
	[Authorize(Roles = "Teacher, Admin")]
	public async Task<IActionResult> Create()
	{
        var gradesDropDownsData = await _gradeService.GetGradesDropdownsData();
		ViewBag.Students = new SelectList(gradesDropDownsData.Students, "Id", "FullName");
		ViewBag.Subjects = new SelectList(gradesDropDownsData.Subjects, "Id", "Name");
		return View();
	}
	[Authorize(Roles = "Teacher, Admin")]
	[HttpPost]
    public async Task<IActionResult> Create(GradeDTO gradeDTO)
    {
        if (ModelState.IsValid)
        {
			await _gradeService.CreateAsync(gradeDTO);
			return RedirectToAction("Index");
		}

		var gradesDropDownsData = await _gradeService.GetGradesDropdownsData();
		ViewBag.Students = new SelectList(gradesDropDownsData.Students, "Id", "FullName");
		ViewBag.Subjects = new SelectList(gradesDropDownsData.Subjects, "Id", "Name");
		return View();
    }

    public async Task<IActionResult> Index()
    {
        var allGrades = await _gradeService.GetAllVMsAsync();

        return View(allGrades);
    }
	[Authorize(Roles = "Teacher, Admin")]
	public async Task<IActionResult> Edit(int id)
    {
		var gradesDropDownsData = await _gradeService.GetGradesDropdownsData();
		ViewBag.Students = new SelectList(gradesDropDownsData.Students, "Id", "LastName");
		ViewBag.Subjects = new SelectList(gradesDropDownsData.Subjects, "Id", "Name");

		var gradeToEdit = await _gradeService.GetByIdAsync(id);
        if(gradeToEdit == null)
        {
            return View("NotFound");
        }

        return View(gradeToEdit);
    }
	[Authorize(Roles = "Teacher, Admin")]
	[HttpPost]
    public async Task<IActionResult> Edit(GradeDTO gradeDTO)
    {
        await _gradeService.EditAsync(gradeDTO);
        return RedirectToAction("Index");
    }
	[Authorize(Roles = "Teacher, Admin")]
	[HttpPost]
    public async Task<IActionResult> Delete(GradeDTO gradeDTO)
    {
        await _gradeService.DeleteAsync(gradeDTO);
        return RedirectToAction("Index");
	}
}
