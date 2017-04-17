using NReco.PdfGenerator;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorEngine.Text;
using System;
using System.IO;
using System.Web.Hosting;
using System.Collections.Generic;

namespace Receipt.Hellper
{
    public class PdfGenerator
    {
        private static string GenerateHtml(object model)
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

            Engine.Razor = RazorEngineService.Create(config);

            string body = string.Empty;
            
            body = Engine.Razor.RunCompile("/views/pdf/template.cshtml", model.GetType(), model, null);
            return body;
        }


        public static byte[] GeneratePdf(object model)
        {
            HtmlToPdfConverter converter = new HtmlToPdfConverter();
            converter.Margins = new PageMargins() { Bottom = 5, Top = 5, Left = 5, Right = 5 };
            var html = GenerateHtml(model);
            return converter.GeneratePdf(html);
        }

    }
}




