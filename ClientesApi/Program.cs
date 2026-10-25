using ClientesApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1) Controladores (aquí vive ClientesController)
builder.Services.AddControllers();

// 2) Entity Framework Core apuntando a Azure SQL Database.
//    La cadena se lee de appsettings.json -> "ConnectionStrings:DefaultConnection"
//    (en Azure App Service se puede sobrescribir desde Configuración).
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sql => sql.EnableRetryOnFailure())); // reintenta si Azure SQL estaba "dormido"

// 3) Documento OpenAPI (para Swagger)
builder.Services.AddOpenApi();

// 4) CORS: permite que otras aplicaciones (como Blazor) consuman la API
builder.Services.AddCors(options =>
    options.AddPolicy("PermitirTodo", policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

// Swagger habilitado también en Azure para poder demostrar la API
app.MapOpenApi();                                   // /openapi/v1.json
app.UseSwaggerUI(o =>
{
    o.SwaggerEndpoint("/openapi/v1.json", "API Clientes v1");
    o.RoutePrefix = "swagger";                      // /swagger
});

app.UseHttpsRedirection();
app.UseCors("PermitirTodo");
app.MapControllers();

// Al entrar a la raíz se redirige a Swagger
app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

app.Run();
