using GraphQLDemo.Schema.Filters;
using GraphQLDemo.Schema.Sorters;
using GraphQLDemo.Services;
using GraphQLDemo.Services.Courses;
using HotChocolate;
using HotChocolate.Data;
using Microsoft.EntityFrameworkCore;


namespace GraphQLDemo.Schema.Query
{
    public class Query
    {
        private readonly CoursesRepository coursesRepository;

        public Query(CoursesRepository coursesRepository)
        {
            this.coursesRepository = coursesRepository;
        }

        public async Task<IEnumerable<CourseType>> GetCourses()
        {
            
            var coursesDto =  await coursesRepository.GetCourses();
            return coursesDto.Select(c => new CourseType
            {
                Id = c.Id,
                Name = c.Name,
                Subject = c.Subject,
                InstructorId = c.InstructorId
            });
        }

        //[UseDbContext(typeof(SchoolDbContext))]
        [UsePaging(IncludeTotalCount = true, DefaultPageSize = 10)]
        [UseProjection]
        [UseFiltering(typeof(CourseFilterType))]
        [UseSorting(typeof(CourseSortType))]
        public IQueryable<CourseType> GetPaginatedCourses([Service] SchoolDbContext context)
        {

            return context.Courses.Select(c => new CourseType
            {
                Id = c.Id,
                Name = c.Name,
                Subject = c.Subject,
                InstructorId = c.InstructorId,
                CreatorId = c.CreatorId
            });
        }

        public async Task<CourseType?> GetCourseByIdAsync(Guid id)
        {
            
            var courseDto = await coursesRepository.GetCourseByIdAsync(id);
            if (courseDto != null)
            {
                return new CourseType
                {
                    Id = courseDto.Id,
                    Name = courseDto.Name,
                    Subject = courseDto.Subject,
                    InstructorId = courseDto.InstructorId,
                    CreatorId = courseDto.CreatorId
                };
            }
            else
            { 
                return null;
            }   

        }

        public async Task<IEnumerable<ISearchResultType>> Search(string term, [Service] SchoolDbContext context)
        {
            var courses = (await context.Courses
                .Where(c => c.Name.Contains(term))
                .ToListAsync())
                .Select(c => new CourseType
                {
                    Id = c.Id,
                    Name = c.Name,
                    Subject = c.Subject,
                    InstructorId = c.InstructorId,
                    CreatorId = c.CreatorId
                });

            var instructors = (await context.Instructors
                .Where(i => i.FirstName.Contains(term) || i.LastName.Contains(term))
                .ToListAsync())
                .Select(i => new InstructorType
                {
                    Id = i.Id,
                    FirstName = i.FirstName,
                    LastName = i.LastName,
                    Salary = i.Salary
                });

            return new List<ISearchResultType>()
                .Concat(courses)
                .Concat(instructors);
        }
        [GraphQLDeprecated("this query is deprecated")]
        public string Instruction => "Use the GraphQL endpoint to query data. For example, you can query for a list of users or a specific user by ID.";
    }

}