using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;
using WebApiAutores;
using WebApiAutores.Servicios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().
    AddJsonOptions(x=>x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c => {
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    c.IgnoreObsoleteActions();
    c.IgnoreObsoleteProperties();
    c.CustomSchemaIds(type => type.FullName);
});

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
        )
    );
builder.Services.AddTransient<IServicio, ServicioA>();

builder.Services.AddTransient<ServicioTransient>();
builder.Services.AddScoped<ServicioScoped>();
builder.Services.AddSingleton<ServicioSingelton>();

var app = builder.Build();

// Configure the HTTP request pipeline.


app.Use(async (contexto, siguiente) => {

    using (var ms = new MemoryStream())
    {
        var cuerpooriginalrespuesta = contexto.Response.Body;
        contexto.Response.Body = ms;

        await siguiente.Invoke();

        ms.Seek(0, SeekOrigin.Begin);
        string respuesta = new StreamReader(ms).ReadToEnd();
        ms.Seek(0, SeekOrigin.Begin);

        await ms.CopyToAsync(cuerpooriginalrespuesta);
        contexto.Response.Body = cuerpooriginalrespuesta;
        app.Logger.LogInformation(respuesta);
    }


});




app.Map("/ruta1", app =>
{

    app.Run(async contexto =>
                {
                    await contexto.Response.WriteAsync("Estoy interceptando la tuberia");

                }

    );
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
