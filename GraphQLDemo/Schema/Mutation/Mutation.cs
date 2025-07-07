using FirebaseAdminAuthentication.DependencyInjection.Models;
using GraphQLDemo.DTOs;
using GraphQLDemo.Schema.Subscription;
using GraphQLDemo.Services.Courses;
using HotChocolate.Authorization;
using HotChocolate.Subscriptions;
using System.Security.Claims;
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
        [Authorize]
        public async Task<CourseResult> CreateCourse(CourseInputType courseInputType, [Service] ITopicEventSender topicEventSender, ClaimsPrincipal claimsPrincipal)
        {
            var user_id = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.ID);

            var courseDTO = new CourseDto
            {
                Id = Guid.NewGuid(),
                Name = courseInputType.Name,
                Subject = courseInputType.Subject,
                InstructorId = courseInputType.InstructorId,
                CreatorId = user_id,
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

        [Authorize]
        public async Task<CourseResult> UpdateCourse(Guid id,CourseInputType courseInputType , [Service] ITopicEventSender topicEventSender
            , ClaimsPrincipal claimsPrincipal)
        {
            var user_id = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.ID);
            var currentCourse = await _coursesRepository.GetCourseByIdAsync(id);
            if (currentCourse == null)
            {
                throw new Exception("Course not found");
            }
            if (currentCourse.CreatorId != user_id)
            {
                throw new GraphQLException(new Error("You are not authorized to update this course", "UNAUTHORIZED_UPDATE"));
            }
            currentCourse.Name = courseInputType.Name;
            currentCourse.Subject = courseInputType.Subject;
            currentCourse.InstructorId = courseInputType.InstructorId;
            currentCourse = await _coursesRepository.Update(currentCourse);
            CourseResult courseResult = new CourseResult()
            {
                Id = currentCourse.Id,
                Name = currentCourse.Name,
                Subject = currentCourse.Subject,
                InstructorId = currentCourse.InstructorId
            };

            var updateCourseTopic = $"{courseResult.Id}_CourseUpdated";
            await topicEventSender.SendAsync(updateCourseTopic, courseResult);

            return courseResult;
        }

        [Authorize(Policy ="IsAdmin")]
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
