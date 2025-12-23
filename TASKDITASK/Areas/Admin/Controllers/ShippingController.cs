using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TASKDITASK.Contexts;
using TASKDITASK.Models;

namespace TASKDITASK.Areas.Admin.Controllers;
[Area("Admin")]
public class ShippingController : Controller
{
    private readonly AppDbContext _context;

    public ShippingController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var items = await _context.Areas.ToListAsync();

        return View(items);
    }

    [HttpGet]
    public IActionResult Create() 
    { 
    return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(ShippingArea items)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }


       await _context.Areas.AddAsync(items);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.Areas.FindAsync(id);

        if (item is null)
        {
            return NotFound();
        }

        _context.Areas.Remove(item);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
