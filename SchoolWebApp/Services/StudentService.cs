using Microsoft.EntityFrameworkCore;
using SchoolWebApp.DTO;
using SchoolWebApp.Models;

namespace SchoolWebApp.Services;

public class StudentService
{
    private ApplicationDbContext _dbContext;

    public StudentService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<StudentDTO> GetStudents()
    {
        var studentsDtos = new List<StudentDTO>();
        var allStudents = _dbContext.Students;
        allStudents.ToList().ForEach(student => studentsDtos.Add(new StudentDTO
        {
            DateOfBirth = student.DateOfBirth,
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
        }));

        return studentsDtos;
    }

    public async Task AddStudentAsync(StudentDTO student)
	{
		await _dbContext.Students.AddAsync(DTOToModel(student));

		await _dbContext.SaveChangesAsync();
	}

	private static StudentDTO ModelToDTO(Student student)
	{
		return new StudentDTO()
		{
			FirstName = student.FirstName,
			LastName = student.LastName,
			DateOfBirth = student.DateOfBirth
		};
	}

	private static Student DTOToModel(StudentDTO student)
	{
		return new Student()
		{
			Id= student.Id,
			FirstName = student.FirstName,
			LastName = student.LastName,
			DateOfBirth = student.DateOfBirth
		};
	}

	public async Task<StudentDTO> GetStudentByIdAsync(int id)
	{
        var student = await _dbContext.Students.FirstOrDefaultAsync(student => student.Id == id);
        if (student == null) 
        {
            return null;
        }
        return ModelToDTO(student);

	}

	internal async Task EditSudentAsync(StudentDTO student)
	{
        _dbContext.Students.Update(DTOToModel(student));
        await _dbContext.SaveChangesAsync();
	}

	internal async Task RemoveStudentAsync(int id)
	{
		Student? student = await _dbContext.Students.FirstOrDefaultAsync(student => student.Id == id);
		if (student != null)
		{
			_dbContext.Remove(student);
			await _dbContext.SaveChangesAsync();
		}
	}
}
