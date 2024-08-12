using Microsoft.EntityFrameworkCore;
using SchoolWebApp.DTO;
using SchoolWebApp.Models;
using SchoolWebApp.ViewModels;

namespace SchoolWebApp.Services;

public class GradeService
{
    public ApplicationDbContext _dbContext;

    public GradeService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GradesDropdownsViewModel> GetGradesDropdownsData()
    {
        var gradesDropdownsData = new GradesDropdownsViewModel()
        {
            Students = await _dbContext.Students.OrderBy(student => student.LastName).ToListAsync(),
            Subjects = await _dbContext.Subjects.OrderBy(student => student.Name).ToListAsync()
		};

        return gradesDropdownsData;
    }

	internal async Task CreateAsync(GradeDTO gradeDTO)
	{
		Grade gradeToInsert = await DtoToModel(gradeDTO);
		await _dbContext.Grades.AddAsync(gradeToInsert);
		await _dbContext.SaveChangesAsync();
	}

	private async Task<Grade> DtoToModel(GradeDTO gradeDTO)
	{
		return new Grade()
		{
            Id = gradeDTO.Id,
			Date = DateTime.Today,
			Mark = gradeDTO.Mark,
			Topic = gradeDTO.Topic,
			Student = await _dbContext.Students.FirstOrDefaultAsync(student => student.Id == gradeDTO.StudentId),
			Subject = await _dbContext.Subjects.FirstOrDefaultAsync(subject => subject.Id == gradeDTO.SubjectId)
		};
	}

	public async Task<IEnumerable<GradesViewModel>> GetAllVMsAsync()
    {
        List<GradesViewModel> grades = new();

        await _dbContext.Grades.Include(grade => grade.Student).Include(grade => grade.Subject).ForEachAsync(grade =>
        {
            grades.Add(new GradesViewModel
            {
                Date = grade.Date,
                Mark = grade.Mark,
                Topic = grade.Topic,
                Id = grade.Id,
                StudentName = $"{grade.Student.LastName} {grade.Student.FirstName}",
                SubjectName = grade.Subject.Name,
            });
        });

        return grades;
    }

    public async Task<GradeDTO> GetByIdAsync(int id)
    {
        return ModelToDto(await _dbContext.Grades
            .Include(grade => grade.Subject)
            .Include(grade => grade.Student)
            .FirstOrDefaultAsync(grade => grade.Id == id));
    }

    internal GradeDTO ModelToDto(Grade grade)
    {
		if (grade == null)
		{
			return null;
		}
		return new GradeDTO
        {
            Id = grade.Id,
            Date = grade.Date,
            Mark = grade.Mark,
            StudentId = grade.Student.Id,
            SubjectId = grade.Subject.Id,
            Topic = grade.Topic
        };
    }

	internal async Task EditAsync(GradeDTO gradeDTO)
	{
		Grade grade = await DtoToModel(gradeDTO);
		_dbContext.Grades.Update(grade);
		await _dbContext.SaveChangesAsync();
	}

	internal async Task DeleteAsync(GradeDTO gradeDTO)
	{
		Grade grade = await DtoToModel(gradeDTO);
		_dbContext.Grades.Remove(grade);
		await _dbContext.SaveChangesAsync();
	}
}