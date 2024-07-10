using ASP_MVC_Movie.Data;
using ASP_MVC_Movie.Interfaces;
using ASP_MVC_Movie.Models;
using ASP_MVC_Movie.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// This is services registrations. All service name says for what it is.
// This is services registrations. All service name says for what it is.
// This is services registrations. All service name says for what it is.


builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<ICommentService, CommentService>();  
builder.Services.AddTransient<IUserManagementService, UserManagementService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddControllersWithViews();
// This is for comunicating with database
builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("Movie"));
});


// This is for authorithation

builder.Services.AddIdentity<AppUser, IdentityRole>()
           .AddEntityFrameworkStores<AppDbContext>()
           .AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(options =>
{

    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 1;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";
});



var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

// This is for user roles

using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
    AppDbContext.EnsureRolesCreated(serviceScope.ServiceProvider).Wait();
}




app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Movie}/{action=Index}/{id?}");

app.Run();
