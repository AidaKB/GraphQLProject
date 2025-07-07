using FirebaseAdmin.Auth;
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
        [IsProjected(true)]
        public Guid InstructorId { get; set; }

        [IsProjected(true)]
        public string? CreatorId { get; set; }

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
        public async Task<UserType?> Creator([Service] UserDataLoader userDataLoader)
        {
            if (string.IsNullOrEmpty(CreatorId))
                return null;

            return await userDataLoader.LoadAsync(CreatorId, CancellationToken.None);

        }

    }
}
