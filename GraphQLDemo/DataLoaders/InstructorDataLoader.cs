using GraphQLDemo.DTOs;
using GraphQLDemo.Services.Instructors;

namespace GraphQLDemo.DataLoaders
{
    public class InstructorDataLoader : BatchDataLoader<Guid, InstructorDto>
    {
        private readonly InstructorsRepository instructorsRepository;

        public InstructorDataLoader(InstructorsRepository instructorsRepository, IBatchScheduler batchScheduler,DataLoaderOptions? options = null
        ) : base(batchScheduler, options)
        {
            this.instructorsRepository = instructorsRepository;
        }
        protected override async Task<IReadOnlyDictionary<Guid, InstructorDto>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
        {
            var instructors = await instructorsRepository.GetManyByIds(keys);
            return instructors.ToDictionary(i => i.Id);
        }
    }
}
