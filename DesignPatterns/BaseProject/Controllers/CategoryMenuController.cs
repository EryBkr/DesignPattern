using BaseProject.Composite;
using BaseProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BaseProject.Controllers
{
    //Category ==> BookComposite
    //Book ==> BookComponent
    [Authorize]
    public class CategoryMenuController : Controller
    {
        private readonly AppIdentityDbContext _context;

        public CategoryMenuController(AppIdentityDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            //Cookie den kullanıcı id sini alıyorum
            var userId = User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value;
            //Kullanıcıya ait kategorileri ve kategorilerin kitaplarını aldım
            var categories = await _context.Categories.Include(i => i.Books).Where(i => i.UserId == userId).OrderBy(i => i.Id).ToListAsync();

            var menu = GetMenu(categories: categories, mainCategory: new Category { Name = "TopCategory", Id = 0 }, mainBookComposite: new BookComposite(0, "TopMenu"));
            ViewBag.Menu = menu;

            //DropDown için alıyorum
            ViewBag.SelectList = menu.Components.SelectMany(i => ((BookComposite)i).GetSelectListItems(""));
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(int categoryId, string bookName)
        {
            await _context.Books.AddAsync(new Book { CategoryId = categoryId, Name = bookName });
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //Menü her zaman categorilerden başlar
        //main category ve main book composite soyut kavramlardır,biz temsili olarak veriyoruz ki referans noktamız olsun
        public BookComposite GetMenu(List<Category> categories, Category mainCategory, BookComposite mainBookComposite, BookComposite last = null)
        {
            //Main kategorileri alıyorum ReferenceId==0
            categories.Where(i => i.ReferenceId == mainCategory.Id).ToList().ForEach(categoryItem =>
            {
                //Main kategorileri BookComposite ile dönüyorum (tabii ilk çalıştığı zaman main kategoriler olacak recursive olduğu için ikinci turdan itibaren sub kategorileri aktarmış oluyoruz)
                var bookComposite = new BookComposite(categoryItem.Id, categoryItem.Name);

                //Sıra Geldi Kitaplara
                categoryItem.Books.ToList().ForEach(bookItem =>
                {
                    //Kategorilere kitapları da yerleştiriyorum (BookComposite ve BookComponent aynı interface'i implement ettiği için problem yaşamıyorum)
                    bookComposite.Add(new BookComponent(bookItem.Id, bookItem.Name));
                });

                //Ağacın son düğümüne gelmediysek
                if (last != null)
                {
                    last.Add(bookComposite);
                }
                else
                {
                    mainBookComposite.Add(bookComposite);
                }

                //Recursive çağırım
                GetMenu(categories, categoryItem, mainBookComposite, bookComposite);
            });

            return mainBookComposite;
        }
    }
}
