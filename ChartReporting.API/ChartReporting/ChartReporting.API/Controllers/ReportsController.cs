using ChartReporting.API.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using static System.Collections.Specialized.BitVector32;

namespace ChartReporting.API.Controllers
{
    
    //[Authorize]
    public class ReportsController : ApiController
    {
       
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Get()
        {
            MemoryStream memoryStream = new MemoryStream();
            
            string filename = "Example";

            var doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35);


            byte[] pdfBytes;
            using (MemoryStream output = new MemoryStream())
            {
                PdfWriter wri = PdfWriter.GetInstance(doc, output);
                doc.Open();

                Paragraph header = new Paragraph("My Document") { Alignment = Element.ALIGN_CENTER };
                Paragraph paragraph = new Paragraph("Testing the iText pdf.");
                Phrase phrase = new Phrase("This is a phrase but testing some formatting also. \nNew line here.");
                Chunk chunk = new Chunk("This is a chunk.");

                doc.Add(header);
                doc.Add(paragraph);
                doc.Add(phrase);
                doc.Add(chunk);
            
                List<System.Drawing.Image> images = new List<System.Drawing.Image>();
                string ruta = HostingEnvironment.ApplicationPhysicalPath;
                images.Add(System.Drawing.Image.FromFile(ruta + "\\Images\\Portada.jpg"));
                foreach (var image in images)
                {
                    iTextSharp.text.Image pic = iTextSharp.text.Image.GetInstance(image, System.Drawing.Imaging.ImageFormat.Jpeg);

                    if (pic.Height > pic.Width)
                    {
                        //Maximum height is 800 pixels.
                        float percentage = 0.0f;
                        percentage = 640 / pic.Height;
                        pic.ScalePercent(percentage * 100);
                    }
                    else
                    {
                        //Maximum width is 600 pixels.
                        float percentage = 0.0f;
                        percentage = 540 / pic.Width;
                        pic.ScalePercent(percentage * 100);
                    }

                    //pic.Border = iTextSharp.text.Rectangle.BOX;
                    //pic.BorderColor = iTextSharp.text.BaseColor.BLACK;
                    pic.BorderWidth = 3f;
                    doc.Add(pic);
                    doc.NewPage();
                }

                doc.Close();
                pdfBytes = output.GetBuffer();
            }
            byte[] buffer = pdfBytes;

            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StreamContent(new MemoryStream(buffer));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            response.Content.Headers.ContentLength = buffer.Length;
            ContentDispositionHeaderValue contentDisposition = null;
            if(ContentDispositionHeaderValue.TryParse("inline; filename=" +  filename +".pdf", out contentDisposition))
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

        //void DrawPng(XGraphics gfx, int number)
        //{
        
           
        //    XImage image = XImage.FromFile(@"C:\Users\fcanul\Source\Repos\chart-reporting-service\ChartReporting.API\ChartReporting\ChartReporting.API\Images\Portada.jpg");
           
        //    const double dx = 250, dy = 140;
          
        //    double width = image.PixelWidth * 72 / image.HorizontalResolution;
        //    double height = image.PixelHeight * 72 / image.HorizontalResolution;
           
        //    gfx.DrawImage(image, (dx - width) / 2, (dy - height) / 2, width, height);
           
           
        //}

       

        //public void DrawPage(PdfPage page)
        //{
        //    XGraphics gfx = XGraphics.FromPdfPage(page);

        //    DrawTitle(page, gfx, "Shapes");
        //    DrawPng(gfx, 2);
        //}

        //public void DrawTitle(PdfPage page, XGraphics gfx, string title)
        //{
        //    XRect rect = new XRect(new XPoint(), gfx.PageSize);
        //    rect.Inflate(-10, -15);
        //    XFont font = new XFont("Verdana", 14, XFontStyle.Bold);
        //    gfx.DrawString(title, font, XBrushes.MidnightBlue, rect, XStringFormats.TopCenter);
            
        //    rect.Offset(0, 5);
        //    font = new XFont("Verdana", 8, XFontStyle.Italic);
        //    XStringFormat format = new XStringFormat();
        //    format.Alignment = XStringAlignment.Near;
        //    format.LineAlignment = XLineAlignment.Far;
        //    gfx.DrawString("Created with " + PdfSharp.ProductVersionInfo.Producer, font, XBrushes.DarkOrchid, rect, format);
            
        //    font = new XFont("Verdana", 8);
        //    format.Alignment = XStringAlignment.Center;
        //    gfx.DrawString(s_document.PageCount.ToString(), font, XBrushes.DarkOrchid, rect, format);
            
        //    s_document.Outlines.Add(title, page, true);
        //}

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
