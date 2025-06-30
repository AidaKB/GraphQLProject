using GraphQLDemo.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GraphQLDemo.Services.Courses
{
    public class CoursesRepository
    {
        private readonly IDbContextFactory<SchoolDbContext> contextFactory;

        public CoursesRepository(IDbContextFactory<SchoolDbContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }
        
        public async Task<CourseDto> Create(CourseDto course)
        {
            using (SchoolDbContext context = contextFactory.CreateDbContext())
            {
                context.Courses.Add(course);
                await context.SaveChangesAsync();

                return course;
            }
        }
    }
}
