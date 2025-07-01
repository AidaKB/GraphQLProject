using GraphQLDemo.DTOs;
using GraphQLDemo.Schema;
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
        public async Task<IEnumerable<CourseDto>> GetCourses()
        {
            using (var context = contextFactory.CreateDbContext())
            {
                return await context.Courses
                    .Include(c => c.Instructor)
                    .Include(c => c.Students)
                    .ToListAsync();
            }

        }
        public async Task<CourseDto?> GetCourseByIdAsync(Guid id)
        {

            using (var context = contextFactory.CreateDbContext())
            {
                return await context.Courses
                    .Include(c => c.Instructor)
                    .Include(c => c.Students)
                    .FirstOrDefaultAsync(c => c.Id == id);
            }
        }

        public async Task<CourseDto> Create(CourseDto course)
        {

            using (SchoolDbContext context = contextFactory.CreateDbContext())
            {
                var instructors = await context.Instructors.ToListAsync();
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
