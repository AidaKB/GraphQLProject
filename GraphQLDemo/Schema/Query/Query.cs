using Bogus;
using GraphQLDemo.DTOs;
using GraphQLDemo.Models;
using GraphQLDemo.Services.Courses;

namespace GraphQLDemo.Schema.Query
{
    public class Query
    {
        private readonly CoursesRepository coursesRepository;

        public Query(CoursesRepository coursesRepository)
        {
            this.coursesRepository = coursesRepository;
        }

        public async Task<IEnumerable<CourseDto>> GetCourses()
        {
            
            return await coursesRepository.GetCourses();
        }
        public async Task<CourseDto> GetCourseByIdAsync(Guid id)
        {
            return await coursesRepository.GetCourseByIdAsync(id);

        }
        [GraphQLDeprecated("this query is deprecated")]
        public string Instruction => "Use the GraphQL endpoint to query data. For example, you can query for a list of users or a specific user by ID.";
    }

}