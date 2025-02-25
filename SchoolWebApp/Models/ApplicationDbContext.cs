﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SchoolWebApp.Models;

public class ApplicationDbContext : IdentityDbContext<AppUser>
{
    public DbSet<Student> Students { get; set; }
	public DbSet<Subject> Subjects { get; set; }
    public DbSet<Grade> Grades { get; set; }
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}
