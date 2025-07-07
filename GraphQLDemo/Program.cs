using FirebaseAdmin;
using FirebaseAdminAuthentication.DependencyInjection.Extensions;
using FirebaseAdminAuthentication.DependencyInjection.Models;
using GraphQLDemo.DataLoaders;
using GraphQLDemo.Schema;
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
   .AddInMemorySubscriptions()
   .AddType<CourseType>()
   .AddType<InstructorType>()
   .AddFiltering()
   .AddSorting()
   .AddProjections()
   .AddAuthorization();

builder.Services.AddSingleton(FirebaseApp.Create());
builder.Services.AddFirebaseAuthentication();
builder.Services.AddAuthorization(
    o => o.AddPolicy("IsAdmin",p => p.RequireClaim(FirebaseUserClaimType.EMAIL, "aida.beheshti81@gmail.com")));

string connectionString = builder.Configuration.GetConnectionString("default");

builder.Services.AddPooledDbContextFactory<SchoolDbContext>(o =>
    o.UseSqlite(connectionString));


builder.Services.AddScoped<CoursesRepository>();
builder.Services.AddScoped<InstructorsRepository>();
builder.Services.AddScoped<InstructorDataLoader>();
builder.Services.AddScoped<SchoolDbContext>();
builder.Services.AddScoped<UserDataLoader>();



var app = builder.Build();

app.UseWebSockets();

app.UseAuthentication();

app.MapGraphQL();

app.Run();