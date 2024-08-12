using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolWebApp.DTO;
using SchoolWebApp.Services;
using System.Globalization;
using System.Xml;
namespace SchoolWebApp.Controllers;
[Authorize]
public class FileUploadController : Controller
{
	private StudentService _studentService;

	public FileUploadController(StudentService studentService)
	{
		_studentService = studentService;
	}
	[Authorize(Roles = "Admin")]
	public IActionResult Index()
	{
		return View();
	}
	[Authorize(Roles = "Admin")]
	[HttpPost]
	public async Task<IActionResult> Upload(IFormFile file)
	{
		if (file.Length > 0)
		{
			string filepath = Path.GetFullPath(file.FileName);
			using(var stream = new FileStream(filepath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
				stream.Close();
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(filepath);
				XmlElement root = xmlDocument.DocumentElement;
				
				string[] formats = { "MM/dd/yyyy", "dd/MM/yyyy", "yyyy-MM-dd", "yyyy/MM/dd" , "d.M.yyyy"};
				CultureInfo cultureInfo = CultureInfo.InvariantCulture;
				DateTime DateOfBirth;

				foreach (XmlNode node in root.SelectNodes("/Students/Student"))
				{
					bool success = DateTime.TryParseExact(node.ChildNodes[2].InnerText, formats, cultureInfo, DateTimeStyles.None, out DateOfBirth);

					if (success)
					{
						StudentDTO student = new StudentDTO()
						{
							FirstName = node.ChildNodes[0].InnerText,
							LastName = node.ChildNodes[1].InnerText,
							DateOfBirth = DateOfBirth,
						};
						await _studentService.AddStudentAsync(student);
					}
				}
			}
		}
		return RedirectToAction("Index", "Students");
	}
}
