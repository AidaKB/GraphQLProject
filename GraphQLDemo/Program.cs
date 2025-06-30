using GraphQLDemo.Schema.Mutation;
using GraphQLDemo.Schema.Query;
using GraphQLDemo.Schema.Subscription;
using GraphQLDemo.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGraphQLServer()
   .AddQueryType<Query>()
   .AddMutationType<Mutation>()
   .AddSubscriptionType<Subscription>()
   .AddInMemorySubscriptions();

string connectionString = builder.Configuration.GetConnectionString("default");

builder.Services.AddPooledDbContextFactory<SchoolDbContext>(o =>
    o.UseSqlite(connectionString));

var app = builder.Build();

app.UseWebSockets();

app.MapGraphQL();

app.Run();