using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADA.CompositionRoot.CastleWindsor.Extentions;
using Castle.DynamicProxy;

namespace ADA.CompositionRoot.CastleWindsor.Installer
{
    public class RepositoryInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {

            container.Register(Component
             .For(typeof(ADA.Data.Repositories.Common.IGenericRepository<>))
             .ImplementedBy(typeof(ADA.Data.Repositories.Common.GenericRepository<>))
             .LifestylePerWebRequest());

            var registrations =
                Types
                .FromAssemblyNamed("ADA.Data")
                .InNamespace("ADA.Data", true)
                .Unless(b => b.IsEnum || 
                    !b.GetInterfaces().Any() ||
                    (!b.Namespace.Contains("Data.Repositories")
                    && !b.Namespace.Contains("Data.Context")
                    && !b.Namespace.Contains("Data.UnitOfWork")))
                .WithService
                .FirstNonGenericCoreInterface("ADA.Data")
                .LifestylePerWebRequest();

            container.Register(registrations);
        }
    }
}
