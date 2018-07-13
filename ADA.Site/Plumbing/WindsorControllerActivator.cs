using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace ADA.Site.Plumbing
{
    public class WindsorControllerActivator : IHttpControllerActivator
    {
        IWindsorContainer _container;

        public WindsorControllerActivator(IWindsorContainer container)
        {
            this._container = container;
        }

        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            var controller =
            (IHttpController)this._container.Resolve(controllerType);

            request.RegisterForDispose(
                new Release(() => this._container.Release(controller)));

            return controller;

        }

        /// <summary>
        /// Encaspsule un action excuté lors du décommissionnement de l'objet disposable
        /// </summary>
        private class Release : IDisposable
        {
            private Action _releaseAction;

            public Release(Action releaseAction)
            {
                this._releaseAction = releaseAction;
            }

            public void Dispose()
            {
                this._releaseAction();
            }
        }
    }
}
