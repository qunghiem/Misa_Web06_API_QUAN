

using MISA_WEB06.BL.Base;
using MISA_WEB06.BL.Interface;
using MISA_WEB06.DL.Base;
using MISA_WEB06.DL.Interface;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// ??ng ký BaseBL cho m?i ki?u T
builder.Services.AddScoped(typeof(IBaseBL<>), typeof(BaseBL<>));

// ??ng ký BaseDL cho m?i ki?u T
builder.Services.AddScoped(typeof(IBaseDL<>), typeof(BaseDL<>));

builder.Services.AddCors(options =>
    options.AddPolicy("AllowVue", p =>
        p.WithOrigins("http://localhost:5173", "http://localhost:3000")
         .AllowAnyHeader().AllowAnyMethod()));

builder.Services.AddSwaggerGen(options =>
{
    // 1. Chú thích cho Tiêu ?? l?n (Library Management API)
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "MISA_WEB06.API",
        Version = "v1.0",
        Description = "API qu?n lý nhân viên" // Dòng mô t? nh? phía d??i tiêu ??
    });

    // 2. C?u hình ?? Swagger ??c ???c các dòng chú thích /// <summary>
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowVue");


app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();
