using NReco.PdfGenerator;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorEngine.Text;
using System;
using System.IO;
using System.Web.Hosting;

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
            config.Namespaces.Add("System.Web");
            config.Namespaces.Add("Receipt.Models");
           
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




