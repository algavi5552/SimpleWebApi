using Microsoft.EntityFrameworkCore;
using Simple_REST_API_App;

var builder = WebApplication.CreateBuilder();
//connection to db
string connection = "Server=(localdb)\\mssqllocaldb;Database=applicationdb;Trusted_Connection=True;";
//add ApplicationContext class to app services
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/users", async (ApplicationContext db) => await db.Users.ToListAsync());

app.MapGet("/api/users/{id:int}", async (int id, ApplicationContext db) =>
{
    // get user by id
    User? user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);

    // if user not found, receive 404 status code and error message
    if (user == null) return Results.NotFound(new { message = "User not found" });

    // if user has been found, send his data
    return Results.Json(user);
});

app.MapDelete("/api/users/{id:int}", async (int id, ApplicationContext db) =>
{
    // get user by id
    User? user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);

    // if user not found, receive 404 status code and error message
    if (user == null) return Results.NotFound(new { message = "User not found" });

    // if user has been found, delete his data
    db.Users.Remove(user);
    await db.SaveChangesAsync();
    return Results.Json(user);
});

app.MapPost("/api/users", async (User user, ApplicationContext db) =>
{
    // add user to array
    await db.Users.AddAsync(user);
    await db.SaveChangesAsync();
    return user;
});

app.MapPut("/api/users", async (User userData, ApplicationContext db) =>
{
    // get user by id
    var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userData.Id);

    // if user not found, receive 404 status code and error message
    if (user == null) return Results.NotFound(new { message = "User not found" });

    // if user has been found, modify his data and send back to client
    user.Age = userData.Age;
    user.Name = userData.Name;
    await db.SaveChangesAsync();
    return Results.Json(user);
});

app.Run();