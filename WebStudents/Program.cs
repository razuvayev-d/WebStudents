using Microsoft.AspNetCore.Diagnostics;
using MySql.Data.MySqlClient;
using System.Net;
using WebDB.Controllers;

var builder = WebApplication.CreateBuilder(args);

string connection = builder.Configuration.GetConnectionString("DefaultConnection");
StudentsController.Constring = connection;
builder.Services.AddRazorPages();

builder.Services.AddMvc();


builder.Services.AddControllersWithViews();//.AddRazorRuntimeCompilation();

var app = builder.Build();
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //context.Response.ContentType = "text/html";
        context.Response.ContentType = "text/plain; charset=utf-8";
        await context.Response.WriteAsync("Ой...\r\n");
        await context.Response.WriteAsync("Что-то пошло не так...\r\n");
        await context.Response.WriteAsync("Код ошибки: " + (int)HttpStatusCode.InternalServerError);
    });
});
app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStatusCodePages();
app.UseRouting();

//app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Students}/{action=Index}/{id?}");

app.Run();
