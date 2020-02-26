using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using MetroWebApi.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MetroWebApi.Options;
using MetroWebApi.Services;
using MetroWebApi.Services.Interfaces;

namespace MetroWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            
            #region DBcontext
            services.AddDbContext<ApplicationContext>(options =>
               options.UseSqlServer(
                   Configuration.GetConnectionString("DefaultConnection")
                   ));
            #endregion

            #region Identity
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
                {
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();
            #endregion

            #region Jwt
            var jwtSettings = new JwtSettings();
            Configuration.Bind(nameof(jwtSettings), jwtSettings);
            services.AddSingleton(jwtSettings);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Key)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = false

                };
            });
            #endregion
         
            #region CustomSevices

            services.AddHttpContextAccessor();

            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IMetroUserService, MetroUserService>();
            services.AddTransient<IRailwayService, RailwayService>();
            services.AddTransient<ITicketArchiveService, TicketArchiveService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRolesService, RolesService>();

            #endregion

            services.AddCors();

            services.AddControllers();

            services.AddSwaggerDocumentation();          
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
           
            if (env.IsDevelopment())
            {
                //app.UseSwaggerDocumentation();
                app.UseDeveloperExceptionPage();
            }
            app.UseSwaggerDocumentation();
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
