using AppAny.HotChocolate.FluentValidation;
using ChatAppGraphQl.Authentication;
using ChatAppGraphQl.Data;
using ChatAppGraphQl.Data.CommentData;
using ChatAppGraphQl.Data.CommunityData;
using ChatAppGraphQl.Data.UserData;
using ChatAppGraphQl.Queries.CommunityQueries;
using ChatAppGraphQl.Queries.Core;
using ChatAppGraphQl.Queries.CommentQueries;
using ChatAppGraphQl.Queries.UserQueries;
using ChatAppGraphQl.Services.CommunityRepository;
using ChatAppGraphQl.Services.CommentRepository;
using ChatAppGraphQl.Services.UserRepository;
using ChatAppGraphQl.Validators;
using FirebaseAdmin;
using FluentValidation.AspNetCore;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ChatAppGraphQl.Data.PostData;
using ChatAppGraphQl.Services.PostRepository;
using ChatAppGraphQl.Queries.PostQueries;
using HotChocolate.Data.Filters;
using HotChocolate.Data.Filters.Expressions;
using ChatAppGraphQl.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(typeof(Program).Assembly);

//Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "./firebase_config.json");
//builder.Services.AddSingleton(FirebaseApp.Create());
builder.Services.AddSingleton(FirebaseApp.Create(new AppOptions() {
    Credential = GoogleCredential.FromJson(builder.Configuration["FirebaseConfig"])
}));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddScheme<AuthenticationSchemeOptions, FirebaseAuthenticationHandler>(
        JwtBearerDefaults.AuthenticationScheme, (o) => { });

builder.Services.AddSingleton<IAuthorizationHandler, AdminHandler>();
builder.Services.AddAuthorization(o => o.AddPolicy("IsAdmin", p =>
    p.AddRequirements(new IsAdmin())));

builder.Services.AddPooledDbContextFactory<ApplicationDbContext>(o =>
    o.UseNpgsql(builder.Configuration.GetConnectionString("testDb"))
);

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddSingleton<UserInputValidator>();
builder.Services.AddSingleton<PostMessageInputValidator>();

builder.Services.AddGraphQLServer()
    .RegisterDbContext<ApplicationDbContext>(DbContextKind.Pooled)
        .AddQueryType<Query>()
        .AddMutationType<Mutation>()
        
        .AddTypeExtension<CommentQuery>()
        .AddTypeExtension<CommentMutation>()
        
        .AddTypeExtension<UserMutation>()
        .AddTypeExtension<UserQuery>()
        
        .AddTypeExtension<CommunityMutation>()
        .AddTypeExtension<CommunityQuery>()

        .AddTypeExtension<PostQuery>()
        .AddTypeExtension<PostMutation>()

        .AddSubscriptionType<Subscription>()
        
        .AddType<CommentType>()
        .AddType<CommunityMutationType>()
        
        .AddType<UserType>()
        .AddType<UserMutationType>()

        .AddType<CommunityType>()
        .AddType<CommunityMutationType>()

        .AddType<PostType>()
        .AddType<PostMutationType>()

        .AddType<Response>()
        
        .AddFiltering()
        .AddConvention<IFilterConvention>(
            new FilterConventionExtension(
                x => x.AddProviderExtension(
                    new QueryableFilterProviderExtension(
                        y => y.AddFieldHandler<QueryableStringInvariantEqualsHandler>()
                    )
                )
            )
        )
        .AddSorting()
        .AddProjections()
        .AddAuthorization()
        .AddFluentValidation(o => o.UseDefaultErrorMapper());

builder.Services.AddInMemorySubscriptions();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ICommunityRepository, CommunityRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.

//app.UseHttpsRedirection();

using (IServiceScope scope = app.Services.CreateScope()) {
    IDbContextFactory<ApplicationDbContext> dbContextFactory =
        scope.ServiceProvider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();

    using ApplicationDbContext context = dbContextFactory.CreateDbContext();
    context.Database.Migrate();
}

app.UseAuthentication();

app.UseAuthorization();

app.UseWebSockets();

app.MapGraphQL();

app.Run();