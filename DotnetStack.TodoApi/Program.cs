using DotnetStack.TodoApi;
using DotnetStack.TodoApi.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var store = new DocumentStore
{
    Urls = new[] { "http://localhost:8080" },
    Database = "Todos"
};
store.Initialize();
builder.Services.AddSingleton<IDocumentStore>(store);
builder.Services.AddScoped<ToDoRepository>();
builder.Services.AddScoped<ToDoService>();
builder.Services.AddScoped<CacheService>();
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("api/todo", async ([FromServices] ToDoService toDoService) 
    => await toDoService.GetAllAsync());
    
app.MapPost("api/todo", async ([FromBody] ToDoItem item, [FromServices] ToDoService toDoService) 
    => await toDoService.CreateAsync(item));

app.MapPut("api/todo", async ([FromBody] ToDoItem item, [FromServices] ToDoService toDoService) 
    => await toDoService.UpdateAsync(item));

app.MapGet("api/todo/{id}", async (string id, [FromServices] ToDoService toDoService) 
    => await toDoService.GetByIdAsync(id));

app.MapDelete("api/todo/{id}", async (string id, [FromServices] ToDoService toDoService)
    => await toDoService.DeleteAsync(id));
    
app.Run();