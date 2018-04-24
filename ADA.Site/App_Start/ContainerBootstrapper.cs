using System;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace ADA.Site.App_Start
{
    public class ContainerBootstrapper : IContainerAccessor, IDisposable
    {
        public readonly IWindsorContainer container;

        ContainerBootstrapper(IWindsorContainer container)
        {
            this.container = container;
        }

        public IWindsorContainer Container
        {
            get { return container; }
        }

        public static ContainerBootstrapper Bootstrap()
        {
            var container = new WindsorContainer().
                Install(FromAssembly.This());
            return new ContainerBootstrapper(container);
        }

        public void Dispose()
        {
            Container.Dispose();
        }
    }
}