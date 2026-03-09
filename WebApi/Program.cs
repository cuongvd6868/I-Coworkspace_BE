using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure;
using Infrastructure.Hubs;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// --- 1. CONFIG SERVICES ---

builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        opt.JsonSerializerOptions.WriteIndented = true;
    })
    .AddOData(opt =>
        opt.Select().Filter().OrderBy().Expand().SetMaxTop(100).Count()
           .AddRouteComponents("odata", GetEdmModel()));

builder.Services.AddOpenApi();

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
}).AddEntityFrameworkStores<AppDbContext>();

// JWT Authentication + SignalR Support
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]))
    };

    // QUAN TRỌNG: Cấu hình để SignalR lấy Token từ Query String
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hub"))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

// Dependency Injection
builder.Services.AddScoped<IHostProfileRepository, HostProfileRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IWorkSpaceFavoriteRepository, WorkSpaceFavoriteRepository>();
builder.Services.AddScoped<IWorkSpaceRepository, WorkSpaceRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IPromotionRepository, PromotionRepository>();
builder.Services.AddScoped<ISupportRepository, SupportRepository>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<IBlockedTimeRepository, BlockedTimeRepository>();
builder.Services.AddScoped<IWorkSpaceRoomRepository, WorkSpaceRoomRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();


builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IWorkSpaceFavoriteService, WorkSpaceFavoriteService>();
builder.Services.AddScoped<IHostProfileService, HostProfileService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IPromotionService, PromotionService>();
builder.Services.AddScoped<ISupportService, SupportService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddHttpClient<IAIService, AIService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IWorkSpaceRoomService, WorkSpaceRoomService>();
builder.Services.AddScoped<IReviewService, ReviewService>();

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddScoped<ISendMailService, SendMailService>();

// SignalR & CORS
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // Thay bằng URL React của bạn
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Bắt buộc cho SignalR
    });
});

// --- 2. CONFIG PIPELINE (THỨ TỰ RẤT QUAN TRỌNG) ---

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// CORS phải đặt TRƯỚC Routing và Auth
app.UseCors("AllowAll");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Đăng ký các Hub và Controller
app.MapHub<SupportHub>("/hub/support");
app.MapHub<ChatHub>("/hub/chat");
app.MapControllers();

app.Run();

// OData Model
static IEdmModel GetEdmModel()
{
    var builder = new ODataConventionModelBuilder();
    // builder.EntitySet<WorkSpace>("WorkSpaces");
    return builder.GetEdmModel();
}