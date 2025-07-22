var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://*:8080");

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sender API V1");
    c.RoutePrefix = string.Empty;  
});

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/ping", () => "pong");

app.Run();
