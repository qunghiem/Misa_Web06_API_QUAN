

using MISA_WEB06.BL.Base;
using MISA_WEB06.BL.Interface;
using MISA_WEB06.DL.Base;
using MISA_WEB06.DL.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ??ng ký BaseBL cho m?i ki?u T
builder.Services.AddScoped(typeof(IBaseBL<>), typeof(BaseBL<>));

// ??ng ký BaseDL cho m?i ki?u T
builder.Services.AddScoped(typeof(IBaseDL<>), typeof(BaseDL<>));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
