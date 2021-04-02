using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AdressBook.Hubs;
using AdressBook.Models;
using AdressBook.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AdressBook
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddMvc();
            services.AddControllers();
            services.AddSignalR();
            //services.AddTransient<IContactService, ContactService>();
            services.AddEntityFrameworkNpgsql().AddDbContext<AdressBookContext>(opt => 
            opt.UseNpgsql(Configuration.GetConnectionString("MyConnection")));
            services.AddControllers()
        .AddJsonOptions(ops =>
        {
            ops.JsonSerializerOptions.IgnoreNullValues = true;
            ops.JsonSerializerOptions.WriteIndented = true;
            ops.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            ops.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
            ops.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

           
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ContactsHub>("/contactsHub");
            });
        }
    }
}
