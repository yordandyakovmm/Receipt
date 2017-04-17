using NReco.PdfGenerator;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorEngine.Text;
using System;
using System.IO;
using System.Linq;
using System.Web.Hosting;

namespace Receipt.Hellper
{
    public class Hellper
    {
        public static string GenerateBigNumber()
        {
            Random random = new Random();
            int digits = 40;
            byte[] buffer = new byte[digits / 2];
            random.NextBytes(buffer);
            string result = String.Concat(buffer.Select(x => x.ToString("X2")).ToArray());
            if (digits % 2 == 0)
                return result;
            return result + random.Next(16).ToString("X");

        }

        

    }
}




