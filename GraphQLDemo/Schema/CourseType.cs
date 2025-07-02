using GraphQLDemo.Models;
using GraphQLDemo.Services.Instructors;

namespace GraphQLDemo.Schema
{
    public class CourseType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Subject Subject { get; set; }
        public Guid InstructorId { get; set; }

        [GraphQLNonNullType]
        public async Task<InstructorType> Instructor([Service] InstructorsRepository instructorsRepository)
        {
            var instructorDto = await instructorsRepository.GetInstructorByIdAsync(InstructorId);

            return new InstructorType
            {
                Id = instructorDto?.Id ?? Guid.Empty,
                FirstName = instructorDto?.FirstName ?? string.Empty,
                LastName = instructorDto?.LastName ?? string.Empty,
                Salary = instructorDto?.Salary ?? 0.0
            };
        }
        public IEnumerable<StudentType>? Students { get; set; }
        public string Description()
        {
            return $"{Name} is a course that the subject of that is {Subject}";
        }

    }
}
