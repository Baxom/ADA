using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using ADA.CompositionRoot.CastleWindsor.Extentions;
using System.Linq;
using Castle.Services.Logging.Log4netIntegration;

namespace ADA.CompositionRoot.CastleWindsor.Installer
{
    public class CommonServicesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {

            //var registrations =
            //   Types
            //   .FromAssemblyNamed("ADA.Infrastructure.Services")
            //   .InNamespace("ADA.Infrastructure.Services.Core", true)
            //   .Unless(b => b.IsEnum || !b.GetInterfaces().Any())
            //   .WithService
            //   .AllInterfaces()
            //   .LifestyleSingleton();

            //container.Register(registrations);

            container
               .Register(
                   Component.For<ADA.Infrastructure.Services.Interface.PdfManager.IPdfManager>()
                   .ImplementedBy<ADA.Infrastructure.Services.Core.PdfManager.iTextSharpPdfManager>().LifestylePerWebRequest()
                   );

            container.AddFacility<Castle.Facilities.Logging.LoggingFacility>(b => b.LogUsing<Log4netFactory>().WithConfig("Config/log4net.config"));
            
        }
    }
}
