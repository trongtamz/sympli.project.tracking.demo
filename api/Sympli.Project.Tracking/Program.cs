using Sympli.Project.Tracking.Applications;
using Sympli.Project.Tracking.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var version = builder.Configuration.GetValue<string>("Version");

builder.Services.AddApiServices(builder.Configuration);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint($"/swagger/v{version}/swagger.json", $"Sympil Tracking API {version}");
    });
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(x => x.AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(_ => true)
    .AllowCredentials());
app.UseMiddleware<ApiExceptionMiddleware>();
app.MapGet("/", async context => { await context.Response.WriteAsync($"Sympli Project Tracking Running V{version}"); });
app.MapControllers();

app.Run();