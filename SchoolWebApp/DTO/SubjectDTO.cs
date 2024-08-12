using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SchoolWebApp.DTO;

public class SubjectDTO
{
    public int Id { get; set; }
    [MaxLength(30)]
	[MinLength(3)]
	[NotNull]
    public string? Name { get; set; }
}
