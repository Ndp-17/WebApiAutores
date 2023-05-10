using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using WebApiAutores;
using WebApiAutores.Filtros;
using WebApiAutores.Middlewares;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(
    opciones =>
    {
        opciones.Filters.Add(typeof(FiltroDeExcepcion));
    }

    ).
    AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles).AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
ConfigurationManager configuration = builder.Configuration;
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
builder.Services.AddSwaggerGen(c =>
{
    //c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    //c.IgnoreObsoleteActions();
    //c.IgnoreObsoleteProperties();
    //c.CustomSchemaIds(type => type.FullName);
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPIAutores", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                
                }
            },
            new string[]{ }
        }       
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
        )
    );


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(c => c.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llavejwt"])),
        ClockSkew = TimeSpan.Zero
    });

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

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
