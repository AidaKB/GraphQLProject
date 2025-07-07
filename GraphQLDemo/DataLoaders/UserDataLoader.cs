using FirebaseAdmin;
using FirebaseAdmin.Auth;
using GraphQLDemo.Schema;
using GreenDonut;

namespace GraphQLDemo.DataLoaders
{
    public class UserDataLoader: BatchDataLoader<string, UserType>
    {
        private readonly FirebaseAuth _firebaseAuth;
        private const int MAX_FIREBASE_USERS_BATCH_SIZE = 100;

        public UserDataLoader(
            FirebaseApp firebaseApp,
            IBatchScheduler batchScheduler,
            DataLoaderOptions? options = null)
            : base(batchScheduler, options ?? new DataLoaderOptions
            {
                MaxBatchSize = MAX_FIREBASE_USERS_BATCH_SIZE
            })
        {
            _firebaseAuth = FirebaseAuth.GetAuth(firebaseApp);
        }

        protected override async Task<IReadOnlyDictionary<string, UserType>> LoadBatchAsync(
            IReadOnlyList<string> userIds,
            CancellationToken cancellationToken)
        {
            var identifiers = userIds.Select(uid => new UidIdentifier(uid)).ToList();

            var usersResult = await _firebaseAuth.GetUsersAsync(identifiers);

            var usersDict = usersResult.Users
                .Select(user => new UserType
                {
                    Id = user.Uid,
                    Username = user.DisplayName ?? "Unknown",
                    PhotoUrl = user.PhotoUrl ?? "https://example.com/default-avatar.png"
                })
                .ToDictionary(user => user.Id);

            return usersDict;
        }
    }
}

