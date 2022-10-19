using JwtAuthenticationManager;
using Microsoft.EntityFrameworkCore;
using UserDbContext.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<UserContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddSingleton<JwtTokenHandler>();
var app = builder.Build();

//if (app.Environment.IsDevelopment())
//    {
//       app.UseSwagger();
//       app.UseSwaggerUI();
//    }

    // Configure the HTTP request pipeline.
app.MapControllers();

app.Run();
