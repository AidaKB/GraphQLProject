namespace GraphQLDemo.Schema
{
    public enum Subject
    {
        Mathematics,
        Science,
        Literature,
        History,
        Art
    }
    public class CourseType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Subject Subject { get; set; }

        [GraphQLNonNullType]
        public InstructorType Instructor { get; set; }
        public IEnumerable<StudentType> Students { get; set; }
        public string Description()
        {
            return $"{Name} is a course that the subject of that is {Subject}";
        }

    }
}
