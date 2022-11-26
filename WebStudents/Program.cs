using MySql.Data.MySqlClient;
using WebDB.Controllers;

var builder = WebApplication.CreateBuilder(args);

var constring = new MySqlConnectionStringBuilder()
{
    Server = "172.24.0.2",
    Port = 3306,
    UserID = "root",
    Password = "oracle",
    Database = "mysql"
};
string connection = builder.Configuration.GetConnectionString("DefaultConnection");
StudentsController.Constring = connection;
builder.Services.AddRazorPages();

MySqlConnection conn = new MySqlConnection(constring.ToString());
builder.Services.AddMvc();


builder.Services.AddControllersWithViews();//.AddRazorRuntimeCompilation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Students}/{action=Index}/{id?}");

app.Run();
