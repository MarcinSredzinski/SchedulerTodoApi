using Business.Library.Models;
using Business.Library.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchedulerTodo.DB;
using SchedulerTodo.Middleware;
using SchedulerTodo.Repositories;
using SchedulerTodo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSingleton<TimeProvider>(TimeProvider.System);
builder.Services.AddOpenApi();
builder.Services.AddScoped<IApiKeyService, ApiKeyService>();
builder.Services.AddSingleton<SoftDeleteInterceptor>();
builder.Services.AddDbContext<SqlServerDbContext>(
    (sp, options) => options
        .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        .AddInterceptors(sp.GetRequiredService<SoftDeleteInterceptor>()));


builder.Services.AddScoped<IAppointmentsRepository, AppointmentsRepository>();
builder.Services.AddScoped<IMyTasksRepository, MyTasksRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/done", (IMyTasksRepository tasksRepository) => tasksRepository.GetItemsDone());

app.MapGet("/todo", (IMyTasksRepository tasksRepository) => tasksRepository.GetItemsToDo());

app.MapPost("/todo",
    (IMyTasksRepository tasksRepository, [FromBody] ItemToDoDto item) => tasksRepository.AddItemToDo(item));

app.MapDelete("/todo",
    (IMyTasksRepository tasksRepository, [FromBody] List<ItemToDo> items) => tasksRepository.DeleteItemsToDo(items));

app.MapPut("/todo",
    (IMyTasksRepository tasksRepository, [FromBody] ItemToDo item) => tasksRepository.UpdateItemToDo(item));

app.UseMiddleware<ApiKeyMiddleware>();
app.Run();