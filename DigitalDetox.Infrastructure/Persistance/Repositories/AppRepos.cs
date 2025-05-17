using DigitalDetox.Core.Context;
using DigitalDetox.Core.Entities.Models;
using DigitalDetox.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Infrastructure.Persistance.Repositories
{
    public class AppRepos : GenericRepos<App>, IAppRepos
    {
        private readonly DegitalDbContext _ctx;
        public AppRepos(DegitalDbContext ctx) : base(ctx)
        {
            _ctx = ctx;
        }


        public async Task<App> AddNewApp(string appName, string? webIconUrl = null!)
        {
            var newApp = new App
            {
                Name = appName,
                WebIconUrl = webIconUrl
            };

            await _ctx.Apps.AddAsync(newApp);
            await SaveAsync();

            return newApp;
        }

        public async Task<App?> GetAppByNameAsync(string appName)
        {
            var app = await _ctx.Apps.FirstOrDefaultAsync(a => a.Name == appName);
            return app;
        }
            
    }
}
