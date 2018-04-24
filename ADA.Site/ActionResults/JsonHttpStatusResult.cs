using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ADA.Site.ActionResults
{
    public class JsonHttpStatusResult : ActionResult
    {
        public int StatusCode { get; private set; }
        public object Data { get; private set; }

        public JsonHttpStatusResult()
        {
            StatusCode = (int)HttpStatusCode.OK;
            Data = null;
        }

        public JsonHttpStatusResult(int statusCode)
        {
            StatusCode = statusCode;
            Data = null;
        }

        public JsonHttpStatusResult(object data)
        {
            StatusCode = (int)HttpStatusCode.OK;
            Data = data;
        }

        public JsonHttpStatusResult(int statusCode, object data)
        {
            StatusCode = statusCode;
            Data = data;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Clear();
            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.ContentEncoding = Encoding.UTF8;
            context.HttpContext.Response.StatusCode = StatusCode;
            context.HttpContext.Response.SuppressFormsAuthenticationRedirect = true; //Pour désactiver la redirection vers la page login si StatusCode = 401
            context.HttpContext.Response.Write(JsonConvert.SerializeObject(Data, new Newtonsoft.Json.JsonSerializerSettings
                {
                    ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
                }));
        }
    }
}
