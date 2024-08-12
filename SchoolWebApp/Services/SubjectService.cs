using Microsoft.EntityFrameworkCore;
using SchoolWebApp.DTO;
using SchoolWebApp.Models;
using SchoolWebApp.ViewModels;

namespace SchoolWebApp.Services;

public class SubjectService
{
    private ApplicationDbContext _dbContext;

    public SubjectService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<SubjectDTO> GetSubjects()
    {
        var SubjectsDtos = new List<SubjectDTO>();
        var allSubjects = _dbContext.Subjects;
        allSubjects.ToList().ForEach(Subject => SubjectsDtos.Add(new SubjectDTO
        {
            Id = Subject.Id,
            Name = Subject.Name
        }));

        return SubjectsDtos;
    }

	public SubjectsViewModel GetSubjectsViewModel()
	{
		SubjectsViewModel subjectsViewModel = new SubjectsViewModel();
		var SubjectsDtos = new List<SubjectDTO>();
		var allSubjects = _dbContext.Subjects;
		allSubjects.ToList().ForEach(Subject => SubjectsDtos.Add(new SubjectDTO
		{
			Id = Subject.Id,
			Name = Subject.Name
		}));

		subjectsViewModel.Subjects = allSubjects.OrderByDescending(subject => subject.Id);

		return subjectsViewModel;
	}

	public async Task AddSubjectAsync(SubjectDTO Subject)
	{
		await _dbContext.Subjects.AddAsync(DTOToModel(Subject));

		await _dbContext.SaveChangesAsync();
	}

	private static SubjectDTO ModelToDTO(Subject Subject)
	{
		return new SubjectDTO()
		{
			Name = Subject.Name
		};
	}

	private static Subject DTOToModel(SubjectDTO Subject)
	{
		return new Subject()
		{
			Id= Subject.Id,
			Name = Subject.Name
		};
	}

	public async Task<SubjectDTO> GetSubjectByIdAsync(int id)
	{
        var Subject = await _dbContext.Subjects.FirstOrDefaultAsync(Subject => Subject.Id == id);
        if (Subject == null) 
        {
            return null;
        }
        return ModelToDTO(Subject);

	}

	internal async Task EditSudentAsync(SubjectDTO Subject)
	{
        _dbContext.Subjects.Update(DTOToModel(Subject));
        await _dbContext.SaveChangesAsync();
	}

	internal async Task RemoveSubjectAsync(int id)
	{
		Subject? Subject = await _dbContext.Subjects.FirstOrDefaultAsync(Subject => Subject.Id == id);
		if (Subject != null)
		{
			_dbContext.Remove(Subject);
			await _dbContext.SaveChangesAsync();
		}
	}
}
