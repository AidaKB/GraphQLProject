using GraphQLDemo.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GraphQLDemo.Services.Instructors
{
    public class InstructorsRepository
    {
        private readonly IDbContextFactory<SchoolDbContext> contextFactory;

        public InstructorsRepository(IDbContextFactory<SchoolDbContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }
        public async Task<InstructorDto?> GetInstructorByIdAsync(Guid id)
        {

            using (var context = contextFactory.CreateDbContext())
            {
                return await context.Instructors.FirstOrDefaultAsync(c => c.Id == id);
            }
        }
        public async Task<IEnumerable<InstructorDto>> GetManyByIds(IReadOnlyList<Guid> ids)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                return await context.Instructors.Where(i => ids.Contains(i.Id)).ToListAsync();
            }
        }
    }
}
