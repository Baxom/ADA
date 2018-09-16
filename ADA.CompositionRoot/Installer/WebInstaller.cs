using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADA.CompositionRoot.CastleWindsor.Extentions;

namespace ADA.CompositionRoot.CastleWindsor.Installer
{
    public class WebInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var registrations =
                Types
                .FromAssemblyNamed("ADA.Site")
                .InNamespace("ADA.Site.Controllers", true)
                .Unless(b => b.IsEnum || !b.GetInterfaces().Any() )
                .LifestylePerWebRequest();
            
            container.Register(registrations);

            registrations =
                Types
                .FromAssemblyNamed("ADA.Site")
                .InNamespace("ADA.Site.ApiControllers", true)
                .Unless(b => b.IsEnum || !b.GetInterfaces().Any())
                .LifestylePerWebRequest();

            container.Register(registrations);

          //  InstallModules(container, store);
          //  InstallFilters(container, store);
          //  InstallHandlers(container, store);
        }

   

    }
}
