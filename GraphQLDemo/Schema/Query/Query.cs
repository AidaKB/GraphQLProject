using GraphQLDemo.Services.Courses;
using GraphQLDemo.Services;
using HotChocolate.Data;
using HotChocolate;


namespace GraphQLDemo.Schema.Query
{
    public class Query
    {
        private readonly CoursesRepository coursesRepository;

        public Query(CoursesRepository coursesRepository)
        {
            this.coursesRepository = coursesRepository;
        }

        [UsePaging(IncludeTotalCount = true,DefaultPageSize =10 )]
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
        public async Task<IEnumerable<CourseType>> GetPaginatedCourses([Service] SchoolDbContext context)
        {

            return context.Courses.Select(c => new CourseType
            {
                Id = c.Id,
                Name = c.Name,
                Subject = c.Subject,
                InstructorId = c.InstructorId
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
                    InstructorId = courseDto.InstructorId

                };
            }
            else
            {                 return null;
            }   

        }
        [GraphQLDeprecated("this query is deprecated")]
        public string Instruction => "Use the GraphQL endpoint to query data. For example, you can query for a list of users or a specific user by ID.";
    }

}