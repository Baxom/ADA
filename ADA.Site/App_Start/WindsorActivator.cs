using System;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(ADA.Site.App_Start.WindsorActivator), "PreStart")]
[assembly: ApplicationShutdownMethodAttribute(typeof(ADA.Site.App_Start.WindsorActivator), "Shutdown")]

namespace ADA.Site.App_Start
{
    public static class WindsorActivator
    {
        public static ContainerBootstrapper bootstrapper;

        public static void PreStart()
        {
            bootstrapper = ContainerBootstrapper.Bootstrap();
        }
        
        public static void Shutdown()
        {
            if (bootstrapper != null)
                bootstrapper.Dispose();
        }
    }
}