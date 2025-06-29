using GraphQLDemo.Schema.Mutation;
using HotChocolate.Execution;
using HotChocolate.Subscriptions;

namespace GraphQLDemo.Schema.Subscription
{
    public class Subscription
    {
        [Subscribe]
        public CourseResult CourseCreated([EventMessage] CourseResult course)
        {
            return course;
        }

        [SubscribeAndResolve]
        public ValueTask<ISourceStream<CourseResult>> CourseUpdated(Guid courseId, [Service] ITopicEventReceiver topicEventReceiver)
        {
            string topicName = $"{courseId}_CourseUpdated";
            return topicEventReceiver.SubscribeAsync<CourseResult>(topicName);
        }
    }
}
