using Bogus;

namespace GraphQLDemo.Schema
{
    public class Query
    {
        private readonly Faker<InstructorType> _instructorFaker;
        private readonly Faker<StudentType> _studentFaker;
        private readonly Faker<CourseType> _courseFaker;
        public Query()
        {
            _instructorFaker = new Faker<InstructorType>()
                .RuleFor(i => i.Id, f => Guid.NewGuid())
                .RuleFor(i => i.FirstName, f => f.Name.FirstName())
                .RuleFor(i => i.LastName, f => f.Name.LastName())
                .RuleFor(i => i.Salary, f => (double)f.Random.Decimal(3000, 10000));

            _studentFaker = new Faker<StudentType>()
                .RuleFor(s => s.Id, f => Guid.NewGuid())
                .RuleFor(s => s.FirstName, f => f.Name.FirstName())
                .RuleFor(s => s.LastName, f => f.Name.LastName())
                .RuleFor(s => s.GPA, f => Math.Round(f.Random.Double(0, 4), 2));
            _courseFaker = new Faker<CourseType>()
                .RuleFor(c => c.Id, f => Guid.NewGuid())
                .RuleFor(c => c.Name, f => f.Company.CatchPhrase())
                .RuleFor(c => c.Subject, f => f.PickRandom<Subject>())
                .RuleFor(c => c.Instructor, f => _instructorFaker.Generate())
                .RuleFor(c => c.Students, f => _studentFaker.Generate(f.Random.Int(3, 10)));

        }

        public IEnumerable<CourseType> GetCourses()
        {
            
            return _courseFaker.Generate(10);
        }
        public async Task<CourseType> GetCourseByIdAsync(Guid id)
        {
            await Task.Delay(1000);
            CourseType course = _courseFaker.Generate();
            course.Id = id;
            return course;

        }
        [GraphQLDeprecated("this query is deprecated")]
        public string Instruction => "Use the GraphQL endpoint to query data. For example, you can query for a list of users or a specific user by ID.";
    }

}