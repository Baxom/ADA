using ADA.Data.Model;
using ADA.Data.UnitOfWork;
using ADA.Domain.Constantes;
using ADA.Site.App_Start;
using ADA.Site.Models;
using ADA.Site.Models.Interface;
using ADA.Site.Models.Paginable;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ADA.Site.ModelBinder
{
    /// <summary>
    /// Binder permettant le binfing automatique de différents types +++++++++++++
    /// </summary>
    public class ExtendedModelBinder : DefaultModelBinder
    {
        IUnitOfWork _unitOfWork { get
            {
                return WindsorActivator.bootstrapper.Container.Resolve<IUnitOfWork>();
            }
        }

        protected override void BindProperty(ControllerContext controllerContext,
            ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor)
        {
            if (propertyDescriptor.PropertyType == typeof(TriModel))
            {
                var value = bindingContext.ValueProvider.GetValue(propertyDescriptor.Name);

                Int32 selectedIndice;

                if (Int32.TryParse(controllerContext.HttpContext.Request.QueryString["tri-select"], out selectedIndice))
                { 
                    
                    ITriable itriableModel = bindingContext.Model as ITriable;

                    if (itriableModel != null && itriableModel.Tris.Any( t => t.Key ==selectedIndice))
                    {
                        var tri = itriableModel.Tris.First(t => t.Key == selectedIndice);
                        propertyDescriptor.SetValue(bindingContext.Model, tri);
                    }
                }
            }
            else if (propertyDescriptor.PropertyType == typeof(PaginationViewModel))
            {
                var value = bindingContext.ValueProvider.GetValue(propertyDescriptor.Name);
                var pagination = controllerContext.HttpContext.Request.QueryString["pagination"];

                if (!String.IsNullOrEmpty(pagination))
                {
                    var parseInt = -1;

                    if (Int32.TryParse(pagination, out parseInt))
                    {
                        propertyDescriptor.SetValue(bindingContext.Model, new PaginationViewModel(parseInt));
                    }
                }
            }
           
            //else if (propertyDescriptor.PropertyType == typeof(RechercheRevueViewModel))
            //{
            //    var value = bindingContext.ValueProvider.GetValue(propertyDescriptor.Name);
            //    var pagination = controllerContext.HttpContext.Request.QueryString["pagination"];

            //    if (!String.IsNullOrEmpty(pagination))
            //    {
            //        var parseInt = -1;

            //        if (Int32.TryParse(pagination, out parseInt))
            //        {
            //            propertyDescriptor.SetValue(bindingContext.Model, new PaginationViewModel(parseInt));
            //        }
            //    }
            //}
            else base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if(bindingContext.ModelType == typeof(RechercheFondViewModel))
            {
                var model = base.BindModel(controllerContext, bindingContext) as RechercheFondViewModel;

                var fond = this._unitOfWork.Fonds.Get(b => b.Id == model.Id, null, b => b.InformationRechercheFonds).FirstOrDefault();

                if (fond != null)
                {
                    fond.InformationRechercheFonds.ToList().ForEach(i => model.InformationsFilters.Add(new Tuple<int, string, TypeColonneFond>(i.Id, controllerContext.HttpContext.Request.QueryString[i.Code], i.Type)));
                }

                return model;
            }

            else return base.BindModel(controllerContext, bindingContext);
        }
    }
}
