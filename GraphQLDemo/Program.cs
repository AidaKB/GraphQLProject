using GraphQLDemo.Schema.Mutation;
using GraphQLDemo.Schema.Query;
using GraphQLDemo.Schema.Subscription;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGraphQLServer()
   .AddQueryType<Query>()
   .AddMutationType<Mutation>()
   .AddSubscriptionType<Subscription>()
   .AddInMemorySubscriptions();


var app = builder.Build();

app.UseWebSockets();

app.MapGraphQL();

app.Run();