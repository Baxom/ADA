using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System.Linq;

namespace ADA.CompositionRoot.CastleWindsor.Installer
{
    public class DomainServicesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var registrations =
               Types
               .FromAssemblyNamed("ADA.Domain.Services")
               .InNamespace("ADA.Domain.Services.Core", true)
               .Unless(b => b.IsEnum || !b.GetInterfaces().Any() )
               .WithService
               .AllInterfaces()
               .LifestylePerWebRequest();

            container.Register(registrations);
        }

    }
}
