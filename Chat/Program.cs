using Chat.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
<<<<<<< Updated upstream
builder.Services.AddDbContext<ChatContext>(x => x.UseMySQL("server=localhost;database=chat_db;user=root;password="));
=======
builder.Services.AddDbContext<ChatContext>(x => x.UseMySQL("server=localhost;database=chat_db;user=root;password=root"));
>>>>>>> Stashed changes

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();