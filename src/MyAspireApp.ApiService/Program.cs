using Microsoft.EntityFrameworkCore;
using MyAspireApp.ApiService.Data;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddNpgsqlDbContext<TodoDbContext>("Database");

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.UseSwagger();
app.UseSwaggerUI();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Length)]
            ))
        .ToArray();
    return forecast;
});

app.MapGet("/todo", async (TodoDbContext context, CancellationToken cancellationToken) =>
{
    var data = await context.Todos.ToListAsync(cancellationToken);

    return Results.Ok(data);
});

app.MapPost("/todo", async (CreateTodo create, TodoDbContext context, CancellationToken cancellationToken) =>
{
    var todo = new Todo
    {
        Id = Guid.NewGuid(),
        Title = create.Title,
        IsDone = false
    };
    context.Todos.Add(todo);

    await context.SaveChangesAsync(cancellationToken);

    return Results.Ok();
});

app.MapDefaultEndpoints();

await app.StartAsync();
// await MigrateDatabase(app.Services);

await app.WaitForShutdownAsync();
return;

async Task MigrateDatabase(IServiceProvider services)
{
    await using var scope = services.CreateAsyncScope();

    var dbContext = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
    await dbContext.Database.MigrateAsync();
}

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}