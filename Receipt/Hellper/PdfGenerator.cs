using NReco.PdfGenerator;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorEngine.Text;
using System;
using System.IO;
using System.Web.Hosting;
using System.Collections.Generic;
using PdfSharp.Pdf;
using EO.Pdf;

namespace Receipt.Hellper
{
    public class ReceiptPdfGenerator
    {
        public static object TheArtOfDev { get; private set; }

        private static string ReceiptGenerateHtml(object model)
        {
            var config = new TemplateServiceConfiguration
            {
                EncodedStringFactory = new RawStringFactory(),
                Language = RazorEngine.Language.CSharp,
                TemplateManager = new DelegateTemplateManager(name =>
                {
                    var content = File.ReadAllText(HostingEnvironment.MapPath(name));
                    return content;
                }),
            };
            config.Namespaces.Add("System");
            config.Namespaces.Add("System.Web");
            config.Namespaces.Add("System.Linq");
            config.Namespaces.Add("Receipt.Models");
            config.Namespaces.Add("System.Collections");
            config.Namespaces.Add("System.Collections.Generic");
            config.Namespaces.Add("System.Runtime");

            Engine.Razor = RazorEngineService.Create(config);

            string body = string.Empty;
            
            body = Engine.Razor.RunCompile("/views/pdf/template.cshtml", model.GetType(), model, null);
            return body;
        }


        public static void GeneratePdf(object model , string path)
        {
            var html = ReceiptGenerateHtml(model);
            HtmlToPdf.ConvertHtml(html, path);
            return;
            
        }

      

    }
}




