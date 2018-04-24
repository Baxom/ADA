using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace ADA.Site.Installers
{
    using Plumbing;

    public class ControllersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Install(new ADA.CompositionRoot.CastleWindsor.Installer.CommonServicesInstaller(),
                                new ADA.CompositionRoot.CastleWindsor.Installer.RepositoryInstaller(),
                                new ADA.CompositionRoot.CastleWindsor.Installer.DomainServicesInstaller(),
                                new ADA.CompositionRoot.CastleWindsor.Installer.WebInstaller());

            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container));
        }
    }
}