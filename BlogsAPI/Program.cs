using System.Reflection; 

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//SWAGGER
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1",
        Description = "API Documentation with JWT Authentication"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer {your token}'"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});

//LOGIING
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});

builder.Services.Scan(s =>
    s.FromAssemblies(Assembly.GetExecutingAssembly())
    .AddClasses(c => c.AssignableTo(typeof(IScopedService<>)))
    .AsImplementedInterfaces()
    .WithScopedLifetime()
);

builder.Services.ConfigAppSettings(builder.Configuration);
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));    
builder.Services.AddScoped<IMailingService, MailingService>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
//builder.Services.AddHangfire(x => x.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
//builder.Services.AddHangfireServer();

//DB
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//IDENTITY
builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

//JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.SaveToken = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:JWTSecret"]!)),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["AppSettings:JWTIssuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["AppSettings:JWTAudience"],
            ValidateLifetime = true,
        };
    });

//Authorization
builder.Services.AddAuthorization(options =>
{ 
    options.FallbackPolicy = new AuthorizationPolicyBuilder().
    AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme).
    RequireAuthenticatedUser().
    Build();
});
 
var app = builder.Build();
 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(options => options.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowCredentials());

app.UseAuthentication();

app.UseAuthorization(); 

//app.UseHangfireDashboard("/hfdashboard");

app.MapControllers();

app.Run();
