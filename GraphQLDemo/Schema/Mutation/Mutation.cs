using GraphQLDemo.DTOs;
using GraphQLDemo.Schema.Subscription;
using GraphQLDemo.Services.Courses;
using HotChocolate.Subscriptions;
using System.Threading.Tasks;

namespace GraphQLDemo.Schema.Mutation
{
    public class Mutation
    {
        private readonly CoursesRepository _coursesRepository;
        public Mutation(CoursesRepository coursesRepository)
        {
            _coursesRepository = coursesRepository;
        }
        public async Task<CourseResult> CreateCourse(CourseInputType courseInputType, [Service] ITopicEventSender topicEventSender)
        {
            var courseDTO = new CourseDto
            {
                Id = Guid.NewGuid(),
                Name = courseInputType.Name,
                Subject = courseInputType.Subject,
                InstructorId = courseInputType.InstructorId,
            };

            courseDTO = await _coursesRepository.Create(courseDTO);

            CourseResult courseResult = new CourseResult()
            {
                Id = courseDTO.Id,
                Name = courseDTO.Name,
                Subject = courseDTO.Subject,
                InstructorId = courseDTO.InstructorId
            };
            await topicEventSender.SendAsync("CourseCreated", courseResult);
            return courseResult;
        }
        public async Task<CourseResult> UpdateCourse(Guid id,CourseInputType courseInputType , [Service] ITopicEventSender topicEventSender)
        {
            var courseDTO = new CourseDto
            {
                Id = id,
                Name = courseInputType.Name,
                Subject = courseInputType.Subject,
                InstructorId = courseInputType.InstructorId,
            };
            courseDTO = await _coursesRepository.Update(courseDTO);
            CourseResult courseResult = new CourseResult()
            {
                Id = courseDTO.Id,
                Name = courseDTO.Name,
                Subject = courseDTO.Subject,
                InstructorId = courseDTO.InstructorId
            };

            var updateCourseTopic = $"{courseResult.Id}_CourseUpdated";
            await topicEventSender.SendAsync(updateCourseTopic, courseResult);

            return courseResult;
        }
        public async Task<bool> DeleteCourse(Guid id)
        {
            try
            {
                return await _coursesRepository.Delete(id);
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
