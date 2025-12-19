using lab3_18;
using lab3_18.api;
using lab3_18.api.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddScoped<TcpServer>();

builder.Services.AddDbContext<AppDbContext>();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UserProfileService>();

builder.Services.AddScoped<Worker>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();

// Ініціалізація бази даних
using (var scope = host.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

await host.RunAsync();