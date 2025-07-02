using GraphQLDemo.Schema.Mutation;
using GraphQLDemo.Schema.Query;
using GraphQLDemo.Schema.Subscription;
using GraphQLDemo.Services;
using GraphQLDemo.Services.Courses;
using GraphQLDemo.Services.Instructors;
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

builder.Services.AddScoped<CoursesRepository>();
builder.Services.AddScoped<InstructorsRepository>();


var app = builder.Build();

app.UseWebSockets();

app.MapGraphQL();

app.Run();