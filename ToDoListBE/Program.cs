using Microsoft.EntityFrameworkCore;
using ToDoListBE.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    // Customize DateTime format (for example, "yyyy-MM-dd")
    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactVite", policy =>
    {
        policy.WithOrigins("http://localhost:5173")  // Vite React app URL
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ToDoListDbContext>(options => options.UseInMemoryDatabase(builder.Configuration.GetConnectionString("DB")));
    
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("ReactVite");


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
