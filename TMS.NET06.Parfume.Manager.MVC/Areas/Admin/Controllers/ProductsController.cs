﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TMS.NET06.Parfume.Manager.MVC.Data;
using TMS.NET06.Parfume.Manager.MVC.Data.Models;

namespace TMS.NET06.Parfume.Manager.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly ParfumeShopContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductsController(ParfumeShopContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var parfumeShopContext = _context.Products.Include(p => p.Brand);
            return View(await parfumeShopContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Name,Price,BrandId,Gender,Volume, Overview")] Product product, [Bind] IList<IFormFile> uploadImages, [Bind] IList<IFormFile> uploadImagesSmall)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                // return RedirectToAction(nameof(Index));
            }

            string directoryPath = Path.Combine(_env.WebRootPath, "img", "prod-img", product.ProductId.ToString());
            CopyFiles(directoryPath, uploadImages);

            string directoryPathSmall = Path.Combine(directoryPath, "small");
            CopyFiles(directoryPathSmall, uploadImagesSmall);

            string imagePathRel = String.Concat("~/img/prod-img/", product.ProductId.ToString());
            SaveImagesInModel(product.Images, imagePathRel, uploadImages);

            imagePathRel = String.Concat("~/img/prod-img/", product.ProductId.ToString(), "/small");
            SaveImagesInModel(product.ImagesSmall, imagePathRel, uploadImagesSmall);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                { }

                return RedirectToAction(nameof(Index));
            }

            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "Name", product.BrandId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "Name", product.BrandId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Price,BrandId,Gender,Volume,Overview")] Product product, [Bind] IList<IFormFile> uploadImages, [Bind] IList<IFormFile> uploadImagesSmall, [Bind] List<string> imagesSmall, [Bind] List<string> images)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            string directoryPath = Path.Combine(_env.WebRootPath, "img", "prod-img", id.ToString());
            CopyFiles(directoryPath, uploadImages);

            string directoryPathSmall = Path.Combine(directoryPath, "small");
            CopyFiles(directoryPathSmall, uploadImagesSmall);


            string imagePathRel = String.Concat("~/img/prod-img/", id.ToString());
            SaveImagesInModel(product.Images, imagePathRel, uploadImages, images);

            imagePathRel = String.Concat("~/img/prod-img/", product.ProductId.ToString(), "/small");
            SaveImagesInModel(product.ImagesSmall, imagePathRel, uploadImagesSmall, imagesSmall);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "Name", product.BrandId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }

        private async void CopyFiles(string directoryPath, IList<IFormFile> uploadImages)
        {
            if (uploadImages != null)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(directoryPath);
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }

                foreach (IFormFile file in uploadImages)
                {
                    if (file.Length > 0)
                    {
                        string filePath = Path.Combine(directoryPath, file.FileName);
                        using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                    }
                }
            }
        }

        private void SaveImagesInModel(List<string> imageListOfProduct, string imagePathRel, IList<IFormFile> uploadImages = null, List<string> images = null)
        {
            imageListOfProduct.Clear();
            //firt of all save images that were in model earlier
            if (images != null)
            {
                foreach (string imagepath in images)
                {
                    if (imagepath != null)
                        if (imagepath.Length != 0) imageListOfProduct.Add(imagepath);
                }
            }
            //then save new images
            if (uploadImages != null)
            {
                foreach (IFormFile file in uploadImages)
                {
                    if (file.Length > 0)
                    {
                        imageListOfProduct.Add(imagePathRel + "/" + file.FileName);

                    }
                }
            }
        }

    }
}
