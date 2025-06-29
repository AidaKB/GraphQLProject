namespace GraphQLDemo.Schema
{
    public class Query
    {
        [GraphQLDeprecated("this query is deprecated")]
        public string Instruction => "Use the GraphQL endpoint to query data. For example, you can query for a list of users or a specific user by ID.";
    }
}
