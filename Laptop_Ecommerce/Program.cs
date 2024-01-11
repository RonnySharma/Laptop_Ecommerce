using Laptop_Ecommerce.DataAccess.Data;
using Laptop_Ecommerce.DataAccess.Rpository;
using Laptop_Ecommerce.DataAccess.Rpository.IRepository;
using Laptop_Ecommerce.Twilio;
using Laptop_Ecommerce.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();

builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<TwilioSettings>(builder.Configuration.GetSection("Twilio"));

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<ISMSService, SMSService>();
builder.Services.AddScoped<TwilioSettings>();

builder.Services.ConfigureApplicationCookie(option =>
{
    option.LoginPath = $"/Identity/Account/Login";
    option.AccessDeniedPath = $"/Identity/Account/AccessDenied";
    option.LogoutPath = $"/Identity/Account/Logout";
});
builder.Services.AddAuthentication().AddFacebook(options =>
{
    options.AppId = "612864600430677";
    options.AppSecret = "01a35994c8ceb90f4e198920f0f88f61";
});
builder.Services.AddAuthentication().AddGoogle(options =>
{
    options.ClientId = "500996044302-356c3bc9p4kiv3pjiemqiotmkv9hov1j.apps.googleusercontent.com";
    options.ClientSecret = "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";
});
builder.Services.AddAuthentication().AddTwitter(options =>
{
    options.ConsumerKey = "hwleXBLkGk7LFZTpbID7ypW6Z";
    options.ConsumerSecret = "92GHcBbpET7EpQeJf69xbvH7lzwaCSGTSPXg1MO4h5flUteUgf";
});
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
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
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
