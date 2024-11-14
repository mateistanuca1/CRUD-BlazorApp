using BlazorApp2.Components;
using BlazorApp2.Components.Pages;
using Microsoft.EntityFrameworkCore;
using BlazorStrap;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddBlazorStrap();
builder.Services.AddDevExpressBlazor();
builder.Services.AddMudServices();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddDbContextFactory<Warehouse>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("connectionString")),
    ServiceLifetime.Transient);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
