using Microsoft.EntityFrameworkCore;
using MinimalApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApiDbContext>(opt=>opt.UseInMemoryDatabase("ApiDb"));

var app = builder.Build();

app.MapGet("/groceries",async(ApiDbContext db) => await db.Groceries.ToListAsync());

app.MapPost("/groceries", async (ApiDbContext db, Grocery grocery) =>
   {
       db.Groceries.Add(grocery);
       await db.SaveChangesAsync();
       return Results.Created($"/shoppinglist/{grocery.Id}", grocery);

   });

app.MapGet("/groceries/{id}", async (ApiDbContext db, int id) =>
{
   var g=  await db.Groceries.FindAsync(id);
    return g!=null?Results.Ok(g): Results.NotFound();
});

app.MapDelete("/groceries/{id}", async (ApiDbContext db, int id) =>
{
    var g = await db.Groceries.FindAsync(id);
    if( g != null) 
    {
        db.Groceries.Remove(g);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    return Results.NotFound();
});

app.MapPut("/groceries/{id}", async (ApiDbContext db, Grocery g,int id) =>
{
    var gInDb = await db.Groceries.FindAsync(id);
    if(gInDb != null)
    {
        gInDb.Name = g.Name;
        gInDb.Purchased=g.Purchased;
        await db.SaveChangesAsync();
        return Results.Ok(gInDb);
    }
    return Results.NotFound();

});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
