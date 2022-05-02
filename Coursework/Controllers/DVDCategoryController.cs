using Coursework.Data;
using Coursework.Models;
using Microsoft.AspNetCore.Mvc;

namespace Coursework.Controllers;

public class DVDCategoryController : Controller
{
    private ApplicationDbContext _context;

    public DVDCategoryController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    // GET All Categories
    public IActionResult Index()
    {
        IEnumerable<DVDCategory> dvdCategories = _context.DvdCategories;
        return View(dvdCategories);
    }
    
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(DVDCategory dvdCategory)
    {
        if (ModelState.IsValid)
        {
            _context.DvdCategories.Add(dvdCategory);
            _context.SaveChanges();
            TempData["success"] = "DVD Category Created Successfully.";
            return RedirectToAction("Index");
        }

        return View(dvdCategory);
    }
    
    [HttpGet]
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var dvdCategory = _context.DvdCategories.Find(id);
        if (dvdCategory == null)
        {
            return NotFound();
        }
        return View(dvdCategory);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(DVDCategory dvdCategory)
    {
        if (ModelState.IsValid)
        {
            _context.DvdCategories.Update(dvdCategory);
            _context.SaveChanges();
            TempData["success"] = "DVD Category Updated Successfully.";
            return RedirectToAction("Index");
        }
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var dvdCategory = _context.DvdCategories.Find(id);
        if (dvdCategory == null)
        {
            return NotFound();
        }
        return View(dvdCategory);
    }
    
    [HttpPost,ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteData(int? id)
    {
        var dvdCategory = _context.DvdCategories.Find(id);
        if (dvdCategory==null)
        {
            return NotFound();
        }

        _context.DvdCategories.Remove(dvdCategory);
        _context.SaveChanges();
        TempData["success"] = "DVD Category Deleted Successfully.";
        return RedirectToAction("Index");
    }
}