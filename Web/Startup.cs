using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;
using Vue.Models;
using VueCliMiddleware;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;

namespace Dashboard
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment _env { get; }
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Handle XSRF Name for Header
            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-XSRF-TOKEN";
            });


            services.AddDbContext<LotiContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CONINFO")));

            //services.Configure<Settings>(Configuration.GetSection("Settings"));

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddControllers().AddNewtonsoftJson(x =>
                    x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            var policy = new AuthorizationPolicyBuilder()
                      .RequireAuthenticatedUser()
                      .Build();

            //  services.AddAuthentication(OAuthValidationDefaults.AuthenticationScheme)

            services.AddAuthentication(o =>
            {
                o.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                o.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                o.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            })
            .AddCookie(options =>
            {
                //options.AccessDeniedPath = new PathString("/Login");
                //options.LoginPath = new PathString("/Login");
                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.RedirectUri = "/Login";
                    return Task.FromResult(0);
                };
                options.Events.OnRedirectToLogin = context =>
                {
                    context.RedirectUri = "/Login";
                    return Task.FromResult(0);
                };
            })

            //.AddCookie(options =>
            //{
            //    options.LoginPath = new PathString("/Login");
            //    options.LogoutPath = new PathString("/Login");
            //    options.AccessDeniedPath = new PathString("/Login");

            //    //Remove the ReturnUrl GET parameter from the sign in page.
            //   options.Events = new CookieAuthenticationEvents()
            //   {
            //       OnRedirectToLogin = redirectContext =>
            //       {
            //           string redirectUri = redirectContext.RedirectUri;

            //           UriHelper.FromAbsolute(
            //               redirectUri,
            //               out string scheme,
            //               out HostString host,
            //               out PathString path,
            //               out QueryString query,
            //               out FragmentString fragment);

            //           redirectUri = UriHelper.BuildAbsolute(scheme, host, path);

            //           redirectContext.Response.Redirect(redirectUri);

            //           return Task.CompletedTask;
            //       }
            //   };
            //})


            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("thisisasecreteforauth")),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromHours(5)
                };
            });



            // For refreshing view pages
            services.AddRazorPages().AddRazorRuntimeCompilation();
            //services.AddDataProtection();

            services.AddMvc(config =>
            {
                config.Filters.Add(new AuthorizeFilter(policy));
                config.EnableEndpointRouting = false;

            });

            //services.AddControllersWithViews(config =>
            //{  
            //});

            services.AddSpaStaticFiles(configuration =>
            {
                //development
                configuration.RootPath = "ClientApp";
                // publish
                configuration.RootPath = "ClientApp/dist";
            });

            //services.AddControllers(opt =>
            //    opt.Filters.Add(new AuthorizeFilter(policy))
            //    );

            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();



            // services.ConfigureApplicationCookie(options => options.LoginPath = "/LogIn");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IApplicationBuilder app2, IWebHostEnvironment env, IAntiforgery antiforgery)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            



            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseSpaStaticFiles();
            app.UseAuthorization();
            app.UseAuthentication();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            // app.UseStatusCodePagesWithReExecute("/Login");

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });

            });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });



            app.Map("",
               adminApp =>
               {
                   app.UseSpa(spa =>
                   {
                       if (env.IsDevelopment())
                           spa.Options.SourcePath = "ClientApp";
                       else
                           spa.Options.SourcePath = "dist";
                       if (env.IsDevelopment())
                       {
                           spa.UseVueCli(npmScript: "serve");
                       }
                   });
               }
           );
        }
    }
}
