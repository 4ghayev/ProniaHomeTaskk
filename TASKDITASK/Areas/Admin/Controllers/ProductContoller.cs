using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TASKDITASK.Contexts;
using TASKDITASK.Models;

namespace TASKDITASK.Areas.Admin.Controllers;
[Area("Admin")]
public class ProductController : Controller
{
    private readonly AppDbContext _context;

    public ProductController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _context.Products
            .Include(x => x.Category)
            .ToListAsync();

        return View(products);
    }

    public async Task<IActionResult> Create()
    {
        var categories = await _context.Categories.ToArrayAsync();


        ViewBag.Categories = categories;
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product product)
    {
       
        if (!ModelState.IsValid)
        {
            var categories = await _context.Categories.ToArrayAsync();
            ViewBag.Categories = categories;
            return View(product);

        }
        var isExistCategory = await _context.Categories.AnyAsync(x => x.Id == product.CategoryId);

        if (!isExistCategory)
        {
            ModelState.AddModelError( "CategoryId","Bele bir category movcud deyil");
            return View(product);
        }
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var categories = await _context.Categories.ToArrayAsync();
        ViewBag.Categories = categories;

        var product = await _context.Products.FindAsync(id);

        if (product is null)
            return NotFound();

        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(Product product)
    {
        if (!ModelState.IsValid)
        {
            var categories = await _context.Categories.ToArrayAsync();
            ViewBag.Categories = categories;
            return View(product);
        }

        var existProduct = await _context.Products.FindAsync(product.Id);

        if (existProduct is null)
        {
            return BadRequest();
        }

        var isExistCategory = await _context.Categories.AnyAsync(x => x.Id == product.CategoryId);

        if (!isExistCategory)
        {
            var categories = await _context.Categories.ToArrayAsync();
            ViewBag.Categories = categories;

            ModelState.AddModelError( "CategoryId",  "Bele bir category movcud deyil");
            return View(product);
        }
        existProduct.Name = product.Name;
        existProduct.Description = product.Description;
        existProduct.Price = product.Price;
        existProduct.ImagePath = product.ImagePath;
        existProduct.CategoryId = product.CategoryId;

        _context.Products.Update(existProduct);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.Products.FindAsync(id);

        if (item is null)
        {
            return NotFound();
        }

        _context.Products.Remove(item);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


}