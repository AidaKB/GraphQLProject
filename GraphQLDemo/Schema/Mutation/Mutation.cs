namespace GraphQLDemo.Schema.Mutation
{
    public class Mutation
    {
        private readonly List<CourseResult> _courses;
        public Mutation()
        {
            _courses = new List<CourseResult>();
        }
        public CourseResult CreateCourse(CourseInputType courseInputType)
        {
            var courseResult = new CourseResult
            {
                Id = Guid.NewGuid(),
                Name = courseInputType.Name,
                Subject = courseInputType.Subject,
                InstructorId = courseInputType.InstructorId
            };
            _courses.Add(courseResult);
            return courseResult;
        }
        public CourseResult UpdateCourse(Guid id,CourseInputType courseInputType)
        {
            var courseResult = _courses.FirstOrDefault(c => c.Id == id);

            if (courseResult==null)
            {
                throw new GraphQLException(new Error("Course Not Found!", "COURSE_NOT_FOUND"));
            }
            courseResult.Name = courseInputType.Name;
            courseResult.Subject = courseInputType.Subject;
            courseResult.InstructorId = courseInputType.InstructorId;
            return courseResult;
        }
        public bool DeleteCourse(Guid id)
        {
            return _courses.RemoveAll(c => c.Id == id) >= 1;
        }

    }
}
