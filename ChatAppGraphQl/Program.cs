using ChatAppGraphQl.Data;
using ChatAppGraphQl.DataLoaders;
using ChatAppGraphQl.Services.MessageRepository;
using ChatAppGraphQl.Services.UserRepository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddPooledDbContextFactory<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("testDb"))
);

builder.Services.AddGraphQLServer()
    .RegisterDbContext<ApplicationDbContext>(DbContextKind.Pooled)
        .AddQueryType<Query>()
        .AddMutationType<Mutation>()
        .AddTypeExtension<MessageQuery>()
        .AddTypeExtension<MessageMutation>()
        .AddTypeExtension<UserMutation>()
        .AddSubscriptionType<Subscription>()
        .AddFiltering()
        .AddSorting()
        .AddProjections()
        .AddAuthorization();

builder.Services.AddInMemorySubscriptions();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<MessageDataLoader>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

using (IServiceScope scope = app.Services.CreateScope()) {
    IDbContextFactory<ApplicationDbContext> dbContextFactory =
        scope.ServiceProvider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();

    using (ApplicationDbContext context = dbContextFactory.CreateDbContext()) {
        context.Database.Migrate();
    }
}

app.UseAuthentication();

app.UseWebSockets();

app.MapGraphQL();

app.Run();