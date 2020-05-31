using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Tweetbook.Installers
{
    public static class InstallerExtensions
    {
        /// <summary>
        /// Configures <paramref name="services"/> with installers defined in the assembly
        /// </summary>
        /// <remarks>
        /// We can use reflection to easily access installers derived from a certain base type/interface, and cleanly install services with an implementation such as the one
        /// shown below. Responsibilities for different domains can then be encapsulated in each installer class (ex. <see cref="MvcInstaller"/>, <see cref="DbInstaller"/>).
        /// </remarks>
        public static void InstallServicesInAssembly(this IServiceCollection services, IConfiguration configuration)
        {
            var installers = typeof(Startup).Assembly.ExportedTypes
                .Where(x => typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(Activator.CreateInstance)
                .Cast<IInstaller>()
                .ToList();

            installers.ForEach(installer => installer.InstallServices(services, configuration));
        }
    }
}
