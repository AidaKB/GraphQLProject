using GraphQLDemo.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

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
        public async Task<CourseDto> Update(CourseDto course)
        {
            using (SchoolDbContext context = contextFactory.CreateDbContext())
            {
                context.Courses.Update(course);
                await context.SaveChangesAsync();

                return course;
            }
        }
        public async Task<bool> Delete(Guid id)
        {
            using (SchoolDbContext context = contextFactory.CreateDbContext())
            {
                CourseDto course = new CourseDto
                {
                    Id = id
                };
                context.Courses.Remove(course);
                return await context.SaveChangesAsync()>0;

            }
        }


    }
}
