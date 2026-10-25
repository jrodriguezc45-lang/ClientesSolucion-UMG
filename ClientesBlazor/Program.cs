using ClientesBlazor.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuración de servicios esenciales para Blazor Server
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// 2. Configurar el HttpClient leyendo la URL pública de la API desde appsettings.json
var apiUrl = builder.Configuration["ApiUrl"];
if (string.IsNullOrEmpty(apiUrl))
{
    // Respaldo automático por si el archivo appsettings no se lee a tiempo
    apiUrl = "https://azurewebsites.net";
}
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiUrl) });

var app = builder.Build();

// 3. Configurar el pipeline de solicitudes HTTP (Middleware)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 4. Enrutamiento obligatorio para Blazor Server en Azure
app.MapBlazorHub(options =>
{
    // Fuerza Long Polling para saltarse las restricciones de Web Sockets de la capa gratuita
    options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.LongPolling;
});

app.MapFallbackToPage("/_Host");

app.Run();
