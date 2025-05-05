
using FastBuy.Products.Api.Middlewares;

namespace FastBuy.Products.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddApplicactionServices(builder.Configuration)
                        .AddPresentation();
        

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseMiddleware<GlobalExceptionMiddleware>();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
