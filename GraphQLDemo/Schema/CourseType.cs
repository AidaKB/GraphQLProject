using GraphQLDemo.DataLoaders;
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
        public async Task<InstructorType> Instructor([Service] InstructorDataLoader instructorDataLoader)
        {
            var instructorDto = await instructorDataLoader.LoadAsync(InstructorId,CancellationToken.None);

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
