using Application.Abstractions;
using Application.DTO;
using DataAccess.Contexts;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

var cs = builder.Configuration.GetConnectionString("DefaultConnection"); //get from app settings
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(cs));
builder.Services.AddScoped<IPeopleRepository, PeopleRepository>();

var app = builder.Build();

app.MapGet("/api/personas",async(IPeopleRepository repository)=>{
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

app.Run();
