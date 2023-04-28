using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BaseProject.Commands
{
    //Bu arkadaş bir Receiver'dir
    //PDF oluşturma işleminden sorumlu arkadaş
    public class PdfFile<TModel>
    {
        public readonly List<TModel> _list;

        //Kütüphaneye ait bir servisi DI ile ekleyemiyorum (Bu class Startup'a DI ile eklenmeyecek (Design Pattern gereği işte)), ondan dolayı HTTPContext bize servisi sağlayacak
        public readonly HttpContext _context;

        public PdfFile(List<TModel> list, HttpContext context)
        {
            _list = list;
            _context = context;
        }

        public string FileName => $"{typeof(TModel).Name}.pdf";

        public string FileType => "application/octet-stream";

        public MemoryStream Create()
        {
            var type = typeof(TModel);

            //HTML içeriğimiz olacak,ona göre render ediyoruz
            var stringbuilder = new StringBuilder();

            //Kullandığımız PDF kütüphanesi HTML to PDF yapıyor,bizde ondan dolayı bu şekilde  HTML sayfası oluşturuyoruz
            stringbuilder.Append($@"
                <html>
                       <head></head>
                       <body>
                            <div class='text-center'>
                                <h1>{type.Name} Tablo</h1>
                            </div>                        
                            <table class='table table-striped' align='center'>");

            stringbuilder.Append("<tr>");

            //Gelen modelin propertyleri ile kolon isimlerini oluşturuyorum
            type.GetProperties().ToList().ForEach(i =>
            {
                stringbuilder.Append($"<th>{i.Name}</th>");
            });
            stringbuilder.Append("</tr>");

            //Tablonun gövdesini gelen datayla render ediyorum
            _list.ForEach(i =>
            {
                //liste de gelen value'ları alıyorum
                var values = type.GetProperties().Select(propertyInfo => propertyInfo.GetValue(i, null)).ToList();

                stringbuilder.Append("<tr>");
                values.ForEach(value =>
                {
                    stringbuilder.Append($"<td>{value}</td>");
                });
                stringbuilder.Append("</tr>");
            });

            //açtığımız tag'leri kapatıyoruz
            stringbuilder.Append(@"</table>
                    </body>
            </html>");

            //Local CSS File (CDN de verebilirdik)
            var cssFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/lib/bootstrap/dist/css/bootstrap.min.css");

            //Create HTML Doc
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                },
                Objects = {
                    new ObjectSettings() {
                        PagesCount = true,
                        HtmlContent = stringbuilder.ToString(),
                        WebSettings = { DefaultEncoding = "utf-8",UserStyleSheet=cssFilePath },
                        HeaderSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 }
                    }
                }
            };

            //HttpContext aracılığıyla PDF kütüphanesi için gerekli olan servisi alıyorum,
            var converter = _context.RequestServices.GetRequiredService<IConverter>();

            //HTML Doc to PDF
            var bytePDF = converter.Convert(doc);

            //byte PDF'i memoryStream'e ekliyoruz ve dönüyoruz
            return new(bytePDF);
        }
    }
}
