using SchoolWebApp.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolWebApp.DTO;

public class GradeDTO
{
    public int Id { get; set; }
    [DisplayName("Student name")]
    public int StudentId { get; set; }
	[DisplayName("Subject name")]
	public int SubjectId { get; set; }
    public string Topic { get; set; }
    public DateTime Date { get; set; }
	[DisplayName("Grade")]
    [Range(1,5)]
	public int Mark { get; set; }
}
