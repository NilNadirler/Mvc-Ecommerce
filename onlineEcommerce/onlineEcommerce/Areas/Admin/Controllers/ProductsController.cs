using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using onlineEcommerce.Data;
using onlineEcommerce.Models;


namespace onlineEcommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private ApplicationDbContext _db;
        private IWebHostEnvironment _he;

        public ProductsController(ApplicationDbContext db, IWebHostEnvironment he)
        {
            _db = db;
            _he = he;
        }

        public IActionResult Index()
        {
            var products = from p in _db.Products
                           join s in _db.SpecialTags on p.SpeacialTagId equals s.Id
                           join t in _db.ProductTypes on p.ProductTypeId equals t.Id
                           select new Product()
                           {
                               Id = p.Id,
                               Name = p.Name,
                               Price = p.Price,
                               Image = p.Image,
                               ProductColor = p.ProductColor,
                               IsAvailable = p.IsAvailable,
                               ProductTypeId = p.ProductTypeId,
                               ProductType = t,
                               SpeacialTagId = p.SpeacialTagId,
                               SpecialTag = s
                           };
            return View(products.ToList());
        }




        public IActionResult Create()
        {
            ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductTypeName");
            ViewData["TagId"] = new SelectList(_db.SpecialTags.ToList(), "Id", "Name");
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Create(Product product, IFormFile image)
        {
            if (ModelState.IsValid)
            {

                var searchProduct = _db.Products.FirstOrDefault(c => c.Name == product.Name);
                if (searchProduct != null)
                {
                    ViewBag.message = "This product is already exist";
                    ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductTypeName");
                    ViewData["TagId"] = new SelectList(_db.SpecialTags.ToList(), "Id", "Name");
                    return View();
                }
                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "\\Images", Path.GetFileName(image.FileName));
                    image.CopyTo(new FileStream(name, FileMode.Create));
                    product.Image = "Images/" + image.FileName;
                }
                if (image == null)
                {
                    product.Image = "Images/noimage.PNG";
                }
                _db.Products.Add(product);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        public ActionResult Edit(int? id)
        {
            ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductTypeName");
            ViewData["TagId"] = new SelectList(_db.SpecialTags.ToList(), "Id", "Name");
            if (id == null)
            {
                return NotFound();
            }
            var product = _db.Products.Include(c => c.ProductType).Include(c => c.SpecialTag)
                .FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product, IFormFile image)
        {
            if (ModelState.IsValid)
            {


                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "Images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    product.Image = "Images/" + image.FileName;
                }
                if (image == null)
                {
                    product.Image = "Images/noimage.PNG";
                }

                _db.Products.Update(product);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();

            }

            var product = _db.Products.Include(c => c.ProductType).Include(c => c.SpecialTag)
                .FirstOrDefault(c => c.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _db.Products.Include(c => c.SpecialTag).Include
                (c => c.ProductType).Where(c => c.Id == id).FirstOrDefault();
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = _db.Products.FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



    }

}
