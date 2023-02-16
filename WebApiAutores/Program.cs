using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;
using WebApiAutores;
using WebApiAutores.Filtros;
using WebApiAutores.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(
    opciones => {
        opciones.Filters.Add(typeof(FiltroDeExcepcion));
    }
    
    ).
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


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

#region Comentado



// Configure the HTTP request pipeline.


//app.Use(async (contexto, siguiente) =>
//{

//    using (var ms = new MemoryStream())
//    {
//        var cuerpooriginalrespuesta = contexto.Response.Body;
//        contexto.Response.Body = ms;

//        await siguiente.Invoke();

//        ms.Seek(0, SeekOrigin.Begin);
//        string respuesta = new StreamReader(ms).ReadToEnd();
//        ms.Seek(0, SeekOrigin.Begin);

//        await ms.CopyToAsync(cuerpooriginalrespuesta);
//        contexto.Response.Body = cuerpooriginalrespuesta;
//        app.Logger.LogInformation(respuesta);
//    }


//});

//app.UseMiddleware<LogRespuestaHTTPMiddleware>();
#endregion
app.UseLogRespuestaHTTP();



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseAuthorization();


app.MapControllers();

app.Run();
app.UseEndpoints(endpoints => endpoints.MapControllers());
