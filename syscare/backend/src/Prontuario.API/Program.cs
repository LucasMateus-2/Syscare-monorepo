using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.IdentityModel.Tokens;
using Prontuario.API.Middleware;
using Prontuario.Application;
using Prontuario.Infrastructure;
using Prontuario.Infrastructure.Services;
using DotNetEnv;
using System.Text.Json;


var builder = WebApplication.CreateBuilder(args);

const string CorsPolicyName = "FrontendPolicy";

builder.WebHost.UseUrls("http://localhost:5000");

// --- Camadas (Domain não precisa de registro; Application/Infrastructure trazem seus casos de uso e serviços) ---
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// --- Controllers + JSON (nomes de propriedade controlados explicitamente via [JsonPropertyName] nos DTOs) ---
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- Limite de upload (fotos) ---
var tamanhoMaximoUploadBytes = builder.Configuration.GetValue<long?>("Upload:TamanhoMaximoBytes") ?? 12 * 1024 * 1024;
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = tamanhoMaximoUploadBytes + 1024 * 1024; // margem para os demais campos do form
});
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = tamanhoMaximoUploadBytes + 2 * 1024 * 1024;
});

// --- CORS: equivalente a cors({ origin: true, credentials: true }) do backend original ---
var origensPermitidas = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
    ?? new[] { "http://localhost:5173" };

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicyName, policy =>
    {
        policy.WithOrigins(origensPermitidas)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// --- Autenticação JWT lida a partir do cookie httpOnly "token" (equivalente ao middleware autenticar/autenticarOpcional) ---
var jwtSecret = builder.Configuration["Jwt:Secret"]
    ?? throw new InvalidOperationException("Configuração 'Jwt:Secret' não definida.");


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "Prontuario.API",
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Issuer"] ?? "Prontuario.API",
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30),
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.TryGetValue("token", out var token) && !string.IsNullOrEmpty(token))
                    context.Token = token;
                return Task.CompletedTask;
            },
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Trata AppException e demais erros e responde no formato { "erro": "mensagem" }, como o backend original.
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCors(CorsPolicyName);

// Serve os arquivos enviados (fotos de pacientes/feridas) em /uploads/..., como o
// `express.static` do backend original.
var uploadRootPath = builder.Configuration["Upload:RootPath"] ?? "uploads";
var uploadFullPath = Path.IsPathRooted(uploadRootPath)
    ? uploadRootPath
    : Path.Combine(app.Environment.ContentRootPath, uploadRootPath);
Directory.CreateDirectory(uploadFullPath);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(uploadFullPath),
    RequestPath = "/uploads",
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
