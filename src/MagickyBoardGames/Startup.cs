using FluentValidation;
using FluentValidation.AspNetCore;
using MagickyBoardGames.Builders;
using MagickyBoardGames.Contexts;
using MagickyBoardGames.Contexts.CategoryContexts;
using MagickyBoardGames.Contexts.GameContexts;
using MagickyBoardGames.Contexts.PlayerContexts;
using MagickyBoardGames.Data;
using MagickyBoardGames.Models;
using MagickyBoardGames.Repositories;
using MagickyBoardGames.Services;
using MagickyBoardGames.Validations;
using MagickyBoardGames.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MagickyBoardGames {
    public class Startup {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env) {
            var builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath).AddJsonFile("appsettings.json", true, true).AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);

            if (env.IsDevelopment())
                builder.AddUserSecrets();

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            services.AddMvc().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            services.AddTransient<IBuilder<Category, CategoryViewModel>, CategoryBuilder>();
            services.AddTransient<IBuilder<Game, GameViewModel>, GameBuilder>();
            services.AddTransient<IBuilder<ApplicationUser, OwnerViewModel>, OwnerBuilder>();
            services.AddTransient<IBuilder<ApplicationUser, PlayerViewModel>, PlayerBuilder>();

            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IGameRepository, GameRepository>();
            services.AddTransient<IGameCategoryRepository, GameCategoryRepository>();
            services.AddTransient<IGameOwnerRepository, GameOwnerRepository>();
            services.AddTransient<IUserRepository, UserRepository>();

            services.AddTransient<IValidator<CategoryViewModel>, CategoryViewModelValidator>();
            services.AddTransient<IValidator<GameSaveViewModel>, GameSaveViewModelValidator>();

            services.AddTransient<ICategoryListContext, CategoryListContext>();
            services.AddTransient<ICategoryViewContext, CategoryViewContext>();
            services.AddTransient<ICategorySaveContext, CategorySaveContext>();
            services.AddTransient<ICategoryContextLoader, CategoryContextLoader>();

            services.AddTransient<IGameListContext, GameListContext>();
            services.AddTransient<IGameViewContext, GameViewContext>();
            services.AddTransient<IGameSaveContext, GameSaveContext>();
            services.AddTransient<IGameContextLoader, GameContextLoader>();

            services.AddTransient<IPlayerListContext, PlayerListContext>();
            services.AddTransient<IPlayerViewContext, PlayerViewContext>();
            services.AddTransient<IPlayerContextLoader, PlayerContextLoader>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            } else {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentity();

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes => { routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}"); });
        }
    }
}