using ChartReporting.API.Models;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Mvc;

namespace ChartReporting.API.Controllers
{
    //[Authorize]
    public class ReportsController : ApiController
    {
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Get()
        {
            MemoryStream memoryStream = new MemoryStream();
            byte[] buffer = memoryStream.GetBuffer();

            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StreamContent(new MemoryStream(buffer));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            response.Content.Headers.ContentLength = buffer.Length;
            ContentDispositionHeaderValue contentDisposition = null;
            if(ContentDispositionHeaderValue.TryParse("inline; filename=testChart.pdf", out contentDisposition))
            {
                response.Content.Headers.ContentDisposition = contentDisposition;
            }

            //Report model = new Report();
            //var template = new LoadedTemplateSource("hell", "~/Views/Reports/Index.cshtml");
            //string html = Engine.Razor.RunCompile(template, "Index", modelType: typeof(Report), model: model);

            //var htmlReponse = Request.CreateResponse(HttpStatusCode.OK);
            //htmlReponse.Content = new StringContent(html);
            //htmlReponse.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");

            //return htmlReponse;
            return response;
        }

        private string ViewToString()
        {
            ViewDataDictionary viewData = new ViewDataDictionary(new Report());
            TempDataDictionary tempData = new TempDataDictionary();
            ControllerContext controllerContext = new System.Web.Mvc.ControllerContext();
            controllerContext.RouteData = new System.Web.Routing.RouteData();

            controllerContext.RouteData.Values.Add("controller", "Reports");

            return string.Empty;
        }
    }
}
