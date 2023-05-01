using BaseProject.ChainOfResponsibility;
using BaseProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BaseProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppIdentityDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppIdentityDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> SendEmail()
        {
            var products = await _context.Products.ToListAsync();

            //Zincirleri oluşturdum
            var excelProcessHandler = new ExcelProcessHandler<Product>();
            var zipFileProcessHandler = new ZipFileProcessHandlar<Product>();
            var sendMailProcessHandler = new SendEmailProcessHandler("product.zip");

            //Zincirleri bağlıyorum
            excelProcessHandler.SetNext(zipFileProcessHandler).SetNext(sendMailProcessHandler);

            //Zinciri çalıştıracağım
            excelProcessHandler.Handle(products);

            return View(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
