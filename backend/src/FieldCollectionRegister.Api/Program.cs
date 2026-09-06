using Dapper;
using FieldCollectionRegister.Core.Interfaces;
using FieldCollectionRegister.DataAccess;
using FieldCollectionRegister.Services;

SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Data Source=fieldcollectionregister.db";

var allowedOrigin = builder.Configuration["Cors:AllowedOrigin"] ?? "http://localhost:3000";

// --- DataAccess layer ---
builder.Services.AddSingleton(new SqliteConnectionFactory(connectionString));
builder.Services.AddScoped<IRegionRepository, RegionRepository>();
builder.Services.AddScoped<IEstateRepository, EstateRepository>();
builder.Services.AddScoped<IFieldRepository, FieldRepository>();
builder.Services.AddScoped<IEntryRepository, EntryRepository>();

// --- Services layer ---
builder.Services.AddScoped<IRegionService, RegionService>();
builder.Services.AddScoped<IEstateService, EstateService>();
builder.Services.AddScoped<IFieldService, FieldService>();
builder.Services.AddScoped<IEntryService, EntryService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        // Sandbox-only: a single hardcoded local origin, not "AllowAnyOrigin".
        policy.WithOrigins(allowedOrigin)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Create the SQLite schema and seed sample data on first run.
DbInitializer.Initialize(app.Services.GetRequiredService<SqliteConnectionFactory>());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Frontend");
app.UseAuthorization();
app.MapControllers();

app.Run();

// Exposed for WebApplicationFactory-style integration tests, if you add any later.
public partial class Program { }
