using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using EMPLEADOS.Data;
using EMPLEADOS.DTOs;
using Microsoft.OpenApi.Any; // Add this using directive

var builder = WebApplication.CreateBuilder(args);

// Configurar servicios de controladores y Swagger con versionado
builder.Services.AddControllers();

// Configurar Entity Framework con PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Agregar versionado de API
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

// Configurar Swagger con ejemplos en OpenAPI
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "EMPLEADOS API", Version = "v1" });

    // Proporcionar ejemplos correctamente sin usar OpenApiInteger ni OpenApiFloat
    options.MapType<EmployeeDto>(() => new OpenApiSchema
    {
        Type = "object",
        Properties = new Dictionary<string, OpenApiSchema>
        {
            ["EmployeeId"] = new OpenApiSchema { Type = "integer", Format = "int64", Example = new OpenApiLong(1) },
            ["FirstName"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("John") },
            ["LastName"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("Doe") },
            ["DateOfBirth"] = new OpenApiSchema { Type = "string", Format = "date", Example = new OpenApiString("1990-01-01") },
            ["HireDate"] = new OpenApiSchema { Type = "string", Format = "date", Example = new OpenApiString("2022-05-15") },
            ["Salary"] = new OpenApiSchema { Type = "number", Format = "double", Example = new OpenApiDouble(50000.00) },
            ["DepartmentId"] = new OpenApiSchema { Type = "integer", Format = "int64", Example = new OpenApiLong(1) }
        }
    });
});


var app = builder.Build();

// Configurar el pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        c.RouteTemplate = "api/{documentName}/docs";
    });

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/api/v1/docs", "EMPLEADOS API V1");
        c.RoutePrefix = string.Empty; // Muestra Swagger UI en la raíz
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
