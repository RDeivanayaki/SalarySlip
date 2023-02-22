using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json.Serialization;
using SalarySlip.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//Enable JSON Result
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling=Newtonsoft.Json.ReferenceLoopHandling.Ignore)
    .AddNewtonsoftJson(options =>options.SerializerSettings.ContractResolver
    =new DefaultContractResolver()); //By Deiva

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IMonthlySalaryDetailRepository, MonthlySalaryDetailRepository>(); //By Deiva
builder.Services.AddScoped<IMonthlistRepository, MonthlistRepository>(); //By Deiva
builder.Services.AddScoped<IYearlistRepository, YearlistRepository>(); //By Deiva
builder.Services.AddScoped<ISalaryForMonthYearRepository, SalaryForMonthYearRepository>(); //By Deiva
builder.Services.AddScoped<IUserlistRepository, UserlistRepository>();
builder.Services.AddScoped<IUserLoginDetailRepository,UserLoginDetailRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



//app.UseHttpsRedirection();//By Deiva
//Enable CORS
app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader().WithExposedHeaders("Content-Disposition")); //By Deiva
app.UseAuthorization();

app.MapControllers();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Upload")),
    RequestPath = "/Upload"
});//By Deiva
app.Run();
