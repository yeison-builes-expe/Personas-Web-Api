using Application.Abstractions;
using Application.DTO;
using Application.Services;
using DataAccess.Contexts;
using DataAccess.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];
var cs = builder.Configuration.GetConnectionString("DefaultConnection"); //get from app settings

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

//dependencies
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(cs));
builder.Services.AddScoped<IPeopleRepository, PeopleRepository>();
builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();

//auth
builder.Services.AddAuthorization();

//swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();  // Genera la documentación de la API
    app.UseSwaggerUI();  // Interfaz gráfica interactiva de Swagger
}

app.MapPost("/api/login", (IAuthenticationService authService,UserLoginDTO userLogin) =>
{
    if (userLogin.User == "admin" && userLogin.Password == "admin")
    {
        var token = authService.GenerateToken(userLogin.User);
        return Results.Ok<ResponseDTO>(new ResponseDTO
        {
            data = token,
            status = 200,
            message = "Respuesta ok"
        });
    }
    return Results.Unauthorized();
});

app.MapGet("/api/personas",[Authorize] async(IPeopleRepository repository)=>{
    var personas = await repository.GetAll();
    if (personas.Count() > 0)
    {
        return Results.Ok<ResponseDTO>(new ResponseDTO
        {
            data = personas,
            status = 200,
            message = "Respuesta ok"
        });
    }

    return Results.BadRequest<ResponseDTO>(new ResponseDTO
    {
        data = null,
        status = 400,
        message = "Error, no hay data"
    });
});

app.MapGet("/api/personas/{id}", [Authorize] async (IPeopleRepository repository,int id) => {
    var result = await repository.Get(id);
    if (result == null)
    {
        return Results.BadRequest(new ResponseDTO
        {
            data = null,
            status = 400,
            message = "Error, no existe"
        });
    }

    return Results.Ok(new ResponseDTO
    {
        data = result,
        status = 200,
        message = "Respuesta ok"
    });
});

app.MapPost("/api/personas", async (IPeopleRepository repository, Persona toCreate) => {
    try
    {
        await repository.Create(toCreate);

        return Results.Ok(new ResponseDTO
        {
            data = toCreate,
            status = 200,
            message = "Registrado correctamente"
        });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new ResponseDTO
        {
            data = null,
            status = 400,
            message = "Error al registrar, verifique la información suministrada"
        });
    }
});

app.MapDelete("/api/personas/{id}", async (IPeopleRepository repository, int id) => {
    try
    {
        await repository.Delete(id);

        return Results.Ok(new ResponseDTO
        {
            data = true,
            status = 200,
            message = "Eliminado correctamente"
        });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new ResponseDTO
        {
            data = null,
            status = 400,
            message = "Error al eliminar, verifique la información suministrada"
        });
    }
});

app.MapPut("/api/personas", async (IPeopleRepository repository, Persona toUpdate) => {
    try
    {
        await repository.Update(toUpdate);

        return Results.Ok(new ResponseDTO
        {
            data = toUpdate,
            status = 200,
            message = "Actualizado correctamente"
        });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new ResponseDTO
        {
            data = null,
            status = 400,
            message = "Error al actualizar, verifique la información suministrada"
        });
    }
});

app.Run();
