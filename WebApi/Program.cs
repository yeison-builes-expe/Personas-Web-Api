var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/test",()=>{
    var test = "hola mundo";
    return test;
});

app.Run();
