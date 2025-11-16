using InventoryManagementSystem.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.Context;
using Microsoft.AspNetCore.Identity.UI.Services;
using InventoryManagementSystem.Utilities;
using InventoryManagementSystem.ViewModels;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();// reperesents involment of both controller and view

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddDbContext<SimpleWebAppDBContext>(c => c.UseSqlServer(connectionString));

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders().AddEntityFrameworkStores<SimpleWebAppDBContext>();

builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.Configure<MailDetailsViewModel>(builder.Configuration.GetSection("mailDetails"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();// convert http to https
app.UseStaticFiles();//represents existance of wwwroot folder
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseAuthentication();
app.UseAuthorization();

app.Run();