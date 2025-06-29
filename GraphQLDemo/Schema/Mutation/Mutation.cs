using GraphQLDemo.Schema.Subscription;
using HotChocolate.Subscriptions;
using System.Threading.Tasks;

namespace GraphQLDemo.Schema.Mutation
{
    public class Mutation
    {
        private readonly List<CourseResult> _courses;
        public Mutation()
        {
            _courses = new List<CourseResult>();
        }
        public async Task<CourseResult> CreateCourse(CourseInputType courseInputType, [Service] ITopicEventSender topicEventSender)
        {
            var courseResult = new CourseResult
            {
                Id = Guid.NewGuid(),
                Name = courseInputType.Name,
                Subject = courseInputType.Subject,
                InstructorId = courseInputType.InstructorId
            };
            _courses.Add(courseResult);
            await topicEventSender.SendAsync("CourseCreated", courseResult);
            return courseResult;
        }
        public async Task<CourseResult> UpdateCourse(Guid id,CourseInputType courseInputType , [Service] ITopicEventSender topicEventSender)
        {
            var courseResult = _courses.FirstOrDefault(c => c.Id == id);

            if (courseResult==null)
            {
                throw new GraphQLException(new Error("Course Not Found!", "COURSE_NOT_FOUND"));
            }
            courseResult.Name = courseInputType.Name;
            courseResult.Subject = courseInputType.Subject;
            courseResult.InstructorId = courseInputType.InstructorId;

            var updateCourseTopic = $"{courseResult.Id}_CourseUpdated";
            await topicEventSender.SendAsync(updateCourseTopic, courseResult);

            return courseResult;
        }
        public bool DeleteCourse(Guid id)
        {
            return _courses.RemoveAll(c => c.Id == id) >= 1;
        }

    }
}
