using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project_Ecomm.DataAccess.Data;
using Project_Ecomm_DataAccess.Repository.IRepository;
using Project_Ecomm_DataAccess.Repository;
using Microsoft.AspNetCore.Identity.UI.Services;
using Project_Ecomm_Uitility;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
// builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
// builder.Services.AddScoped<ICoverTypeRepository,CoverTypeRepository>();
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();
builder.Services.ConfigureApplicationCookie(option =>
{
    option.LoginPath = $"/Identity/Account/Login";
    option.AccessDeniedPath = $"/Identity/Account/AccessDenied";
    option.LogoutPath = $"/Identity/Account/logout";
});
builder.Services.AddAuthentication().AddFacebook(option =>
{
    option.AppId = "909533533688275";
    option.AppSecret = "3fa020f90f4cad2f4dcf7f463e7c59c2";
});
builder.Services.AddAuthentication().AddGoogle(option =>
{
    option.ClientId = "488760029527-vc0lcujgrgt6j5a5vcm82dcj3v4jkrv7.apps.googleusercontent.com";
    option.ClientSecret = "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";
});
builder.Services.AddAuthentication().AddTwitter(option =>
{
    option.ConsumerKey = "nsDTOfYM03OSuL1IqEqEEwwRr";
    option.ConsumerSecret = "6sGxgH6cLMp5j2XurocflxFg1QxEECX4qNkYtG1WX7ACmeDJjW";
});
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(30);
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();      

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe")["Secretkey"];
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{Area=Customer}/{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
