using Coursework.Data;
using Coursework.Models;
using Microsoft.AspNetCore.Mvc;

namespace Coursework.Controllers;

public class StudioController : Controller
{
    private ApplicationDbContext _context;

    public StudioController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    // GET All Categories
    public IActionResult Index()
    {
        IEnumerable<Studio> studios = _context.Studios;
        return View(studios);
    }
    
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Studio studio)
    {
        if (ModelState.IsValid)
        {
            _context.Studios.Add(studio);
            _context.SaveChanges();
            TempData["success"] = "Studio Created Successfully.";
            return RedirectToAction("Index");
        }

        return View(studio);
    }
    
    [HttpGet]
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var studio = _context.Studios.Find(id);
        if (studio == null)
        {
            return NotFound();
        }
        return View(studio);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Studio s)
    {
        if (ModelState.IsValid)
        {
            _context.Studios.Update(s);
            _context.SaveChanges();
            TempData["success"] = "Studio Updated Successfully.";
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

        var studio = _context.Studios.Find(id);
        if (studio == null)
        {
            return NotFound();
        }
        return View(studio);
    }
    
    [HttpPost,ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteData(int? id)
    {
        var studio = _context.Studios.Find(id);
        if (studio==null)
        {
            return NotFound();
        }

        _context.Studios.Remove(studio);
        _context.SaveChanges();
        TempData["success"] = "Studio Deleted Successfully.";
        return RedirectToAction("Index");
    }
}