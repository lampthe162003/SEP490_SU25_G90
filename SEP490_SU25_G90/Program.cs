using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.LearningApplicationsRepository;
using SEP490_SU25_G90.vn.edu.fpt.Services.LearningApplicationsService;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.MotobikeCouseRepository;
using SEP490_SU25_G90.vn.edu.fpt.Services.Course;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("MyCnn") ;
builder.Services.AddDbContext<Sep490Su25G90DbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddAutoMapper(typeof(ObjectMapper));
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<Sep490Su25G90DbContext>();
builder.Services.AddRazorPages();
//Repository
builder.Services.AddScoped<IMotobikeCourseRepository, MotobikeCourseRepository>();
//Service
builder.Services.AddScoped<IMotobikeCourseService, MotobikeCourseService>();
builder.Services.AddControllers();
builder.Services.AddScoped<ILearningApplicationRepository, LearningApplicationRepository>();
builder.Services.AddScoped<ILearningApplicationService, LearningApplicationService>();
var app = builder.Build();
app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
