using BaseProject.Commands;
using BaseProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace BaseProject.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppIdentityDbContext _context;

        public ProductController(AppIdentityDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        public async Task<IActionResult> CreateFile(int type)
        {
            var products = await _context.Products.ToListAsync();

            FileCreateInvoker invoker = new();
            FileTypeEnum fileType = (FileTypeEnum)type;

            //Strategy Design Pattern'ı da burada kullanırsak daha Clean bir çözüme ulaşırız
            switch (fileType)
            {
                case FileTypeEnum.Excel:
                    ExcelFile<Product> excelFile = new(products);
                    invoker.SetCommand(new CreateExcelTableActionCommand<Product>(excelFile));
                    break;
                case FileTypeEnum.PDF:
                    PdfFile<Product> pdfFile = new(products, HttpContext);
                    invoker.SetCommand(new CreatePdfTableActionCommand<Product>(pdfFile));
                    break;
                default:
                    break;
            }

            return invoker.CreateFile();
        }

        public async Task<IActionResult> CreateFiles()
        {
            //Receiver class'ının ihtiyacı olan datayı alıyorum
            var products = await _context.Products.ToListAsync();

            //Recevier class'ının instance'ını oluşturuyorum
            ExcelFile<Product> excelFile = new(products);
            PdfFile<Product> pdfFile = new(products, HttpContext);

            //Invoker'ı aldım
            FileCreateInvoker invoker = new();

            //Invoker'ıma potansiyel olarak çalışacak komutlarımı veriyorum
            invoker.AddCommand(new CreateExcelTableActionCommand<Product>(excelFile));
            invoker.AddCommand(new CreatePdfTableActionCommand<Product>(pdfFile));

            //toplu olarak execute etmeye hazır dönüşümü alıyorum
            var filesResult = invoker.CreateFiles();

            using(var memoryStream=new MemoryStream())
            {
                //Gelen dosyaları zipliyorum
                using(var archive=new ZipArchive(memoryStream, ZipArchiveMode.Create))
                {
                    foreach (var result in filesResult)
                    {
                        var fileContent = result as FileContentResult;
                        var zipFile = archive.CreateEntry(fileContent.FileDownloadName);

                        using (var zipEntryStream = zipFile.Open())
                        {
                            //Burada dosyayı zip'e aktif olarak ekliyorum
                            await new MemoryStream(fileContent.FileContents).CopyToAsync(zipEntryStream);
                        }
                    }

                }
                return File(memoryStream.ToArray(), "application/zip", "all.zip");
            }
        }
    }
}
