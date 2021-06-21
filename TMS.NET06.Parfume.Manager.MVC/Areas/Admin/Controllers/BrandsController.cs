using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TMS.NET06.Parfume.Manager.MVC.Data;
using TMS.NET06.Parfume.Manager.MVC.Data.Models;

namespace TMS.NET06.Parfume.Manager.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandsController : Controller
    {
        private readonly ParfumeShopContext _context;
        private readonly IWebHostEnvironment _env;

        public BrandsController(ParfumeShopContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Brands
        public async Task<IActionResult> Index()
        {
            return View(await _context.Brands.ToListAsync());
        }

        // GET: Brands/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands
                .FirstOrDefaultAsync(m => m.BrandId == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        // GET: Brands/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Brands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Brand brand, [Bind] IFormFile uploadLogo, [Bind] IFormFile uploadFrontImage)
        {
            string directoryPath = Path.Combine(_env.WebRootPath, "img", "brand-img");
            CopyFiles(directoryPath, uploadLogo);
            CopyFiles(directoryPath, uploadFrontImage);

            string imagePathRel = "~/img/brand-img";
            SaveImagesInModel(brand, imagePathRel, uploadLogo);
            SaveImagesInModel(brand, imagePathRel, uploadFrontImage, false);

            if (ModelState.IsValid)
            {
                _context.Add(brand);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(brand);
        }

        // GET: Brands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        // POST: Brands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Brand brand, [Bind] IFormFile uploadLogo, [Bind] IFormFile uploadFrontImage) 
        {
            if (id != brand.BrandId)
            {
                return NotFound();
            }

            string directoryPath = Path.Combine(_env.WebRootPath, "img", "brand-img");
            CopyFiles(directoryPath, uploadLogo);
            CopyFiles(directoryPath, uploadFrontImage);

            string imagePathRel = "~/img/brand-img";
            SaveImagesInModel(brand, imagePathRel, uploadLogo);
            SaveImagesInModel(brand, imagePathRel, uploadFrontImage, false);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(brand);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrandExists(brand.BrandId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }

        // GET: Brands/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands
                .FirstOrDefaultAsync(m => m.BrandId == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        // POST: Brands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BrandExists(int id)
        {
            return _context.Brands.Any(e => e.BrandId == id);
        }

        private async void CopyFiles(string directoryPath, IFormFile uploadImage)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(directoryPath);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            if (uploadImage != null)
            {
                if (uploadImage.Length > 0)
                {
                    string filePath = Path.Combine(directoryPath, uploadImage.FileName);
                    using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await uploadImage.CopyToAsync(fileStream);
                    }
                }
            }

        }

        private void SaveImagesInModel(Brand brand, string imagePathRel, IFormFile uploadImage, bool isLogo = true)
        {
            if (uploadImage != null)
            {
                if (uploadImage.Length > 0)
                {
                    if (isLogo)
                        brand.Logo = imagePathRel + "/" + uploadImage.FileName;
                    else
                        brand.FrontImage = imagePathRel + "/" + uploadImage.FileName;
                }
            }

        }
    }
}
