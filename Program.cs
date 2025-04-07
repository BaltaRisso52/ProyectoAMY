using CloudinaryDotNet;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

string connectionString;

if (string.IsNullOrEmpty(databaseUrl))
{
    // Local con Sqlite
    connectionString = new NpgsqlConnectionStringBuilder(builder.Configuration.GetConnectionString("SqliteConexion")).ToString();
}
else
{
    // Railway con Postgres
    var uri = new Uri(databaseUrl);
    var userInfo = uri.UserInfo.Split(':');

    var builderPostgres = new Npgsql.NpgsqlConnectionStringBuilder
    {
        Host = uri.Host,
        Port = uri.Port,
        Username = userInfo[0],
        Password = userInfo[1],
        Database = uri.AbsolutePath.Trim('/'),
        SslMode = Npgsql.SslMode.Require,
        TrustServerCertificate = true
    };

    connectionString = builderPostgres.ToString();
}

// var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL").ToString()
//                       ?? new NpgsqlConnectionStringBuilder(builder.Configuration.GetConnectionString("SqliteConexion")).ToString();

// var connectionString = builder.Configuration.GetConnectionString(
// "SqliteConexion")!.ToString();
builder.Services.AddSingleton<string>(connectionString);
builder.Services.AddSingleton<CloudinaryService>();

builder.Services.AddSingleton(new Cloudinary(new Account(
    builder.Configuration["Cloudinary:CloudName"],
    builder.Configuration["Cloudinary:ApiKey"],
    builder.Configuration["Cloudinary:ApiSecret"]
)));

builder.Services.AddScoped<IProductoRepository, ProductoRepository>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Productos}/{action=Index}/{id?}");


app.Run();
