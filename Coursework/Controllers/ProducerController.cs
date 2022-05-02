using Coursework.Data;
using Coursework.Models;
using Microsoft.AspNetCore.Mvc;

namespace Coursework.Controllers;

public class ProducerController : Controller
{
     private ApplicationDbContext _context;

    public ProducerController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    // GET All Categories
    public IActionResult Index()
    {
        IEnumerable<Producer> producers = _context.Producers;
        return View(producers);
    }
    
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Producer producer)
    {
        if (ModelState.IsValid)
        {
            _context.Producers.Add(producer);
            _context.SaveChanges();
            TempData["success"] = "Producer Created Successfully.";
            return RedirectToAction("Index");
        }

        return View(producer);
    }
    
    [HttpGet]
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var producer = _context.Producers.Find(id);
        if (producer == null)
        {
            return NotFound();
        }
        return View(producer);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Producer p)
    {
        if (ModelState.IsValid)
        {
            _context.Producers.Update(p);
            _context.SaveChanges();
            TempData["success"] = "Producer Updated Successfully.";
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

        var producer = _context.Producers.Find(id);
        if (producer == null)
        {
            return NotFound();
        }
        return View(producer);
    }
    
    [HttpPost,ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteData(int? id)
    {
        var producer = _context.Producers.Find(id);
        if (producer==null)
        {
            return NotFound();
        }

        _context.Producers.Remove(producer);
        _context.SaveChanges();
        TempData["success"] = "Studio Deleted Successfully.";
        return RedirectToAction("Index");
    }
}