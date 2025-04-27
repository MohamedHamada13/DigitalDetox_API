using DigitalDetox.Core.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.Interfaces
{
    public interface IAppRepos : IGenericRepos<App>
    {
        Task<App?> AddNewApp(string appName);
        Task<App?> GetAppByNameAsync(string appName);
    }
}
