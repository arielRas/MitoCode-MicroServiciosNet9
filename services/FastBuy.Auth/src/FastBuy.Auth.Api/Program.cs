using FastBuy.Auth.Api.DataAccess.Identity;
using FastBuy.Auth.Api.Extensions;
using FastBuy.Auth.Api.Middlewares;

namespace FastBuy.Auth.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddApplicationServices(builder.Configuration)
                            .AddHostedService<IdentitySeedHostedService>()
                            .AddPresentation()
                            .AddRazorPages();


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();

            app.UseIdentityServer();

            app.UseHttpsRedirection();

            app.UseMiddleware<GlobalExceptionMiddleware>();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.MapRazorPages();

            app.Run();
        }
    }
}
