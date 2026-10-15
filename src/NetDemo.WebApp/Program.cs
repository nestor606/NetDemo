using System.Text.Json.Serialization;
using NetDemo.Application.TipoMercado;
using NetDemo.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// WebApp is the composition root for Application and Infrastructure.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddScoped<ITipoMercadoService, TipoMercadoService>();

var tipoMercadoDirectory = builder.Configuration["Storage:TipoMercadoDirectory"]
    ?? "App_Data/TipoMercado";
var tipoMercadoPath = Path.GetFullPath(tipoMercadoDirectory, builder.Environment.ContentRootPath);
builder.Services.AddInfrastructure(tipoMercadoPath);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
