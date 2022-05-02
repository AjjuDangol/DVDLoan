using System.Security.Cryptography;
using Coursework.Data;
using Coursework.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coursework.Controllers;

public class DVDTitleController : Controller
{
    private readonly ApplicationDbContext _context;
    
    public DVDTitleController(ApplicationDbContext context)
    {
        _context = context;
    }

    //Question 1
    public IActionResult DVDTitleSearchBYActorLastName(String surname)
    {
        var databaseContext = _context.CastMembers.Include(x => x.Actor).Include(x=>x.DvdTitle);
        var actors = from a in databaseContext select a;
        if (!String.IsNullOrEmpty(surname))
        {
            actors = actors.Where(x => x.Actor.ActorSurname.Contains(surname));
        }

        return View(actors.ToList());
    }
    
    //Question 3
    // public IActionResult SearchDVDByMemberName(string lastName) {
    //     var member = _context.Members.Where(x => x.MemberLastName == lastName).First();
    //     List<Loan> dvdLoan = new List<Loan>();
    //     dvdLoan = _context.Loans.Where(x => x.Member == member
    //                                         && x.DateOut.Date >= DateTime.Now.Date.AddDays(-31) 
    //                                         && x.DateReturned.ToString() == null).ToList();
    //     ViewBag.loanList = dvdLoan;
    //     return View(dvdLoan);
    // }
    
    public async Task<IActionResult> SearchDVDByMemberName(string searchString)
    {
        var results = _context.Members.Include(m => m.Loans)
            .ThenInclude(l=>l.DvdCopy)
            .ThenInclude(c=>c.DvdTitle)
            .Where(m=>m.Loans.All(l=>l.DateOut <= DateTime.UtcNow.AddDays(30)))
            .Where(m =>m.MemberFirstName.Contains(searchString)).FirstOrDefault();
        ViewData["member"] = results;
        if(results == null)
        {
            ViewData["loans"] = new List<Loan>();
        }
        else
        {
            ViewData["loans"] = results.Loans;
        }
        return View();
    }

    //Question 4
    public IActionResult DVDDetails()
    {
        var databaseContext = _context.DvdTitles.Include(x => x.Producer).Include(x => x.Studio)
            .OrderBy(x => x.DateReleased).ToList();
        foreach (var data in databaseContext)
        {
            List<string> actorList = _context.CastMembers
                .Where(x => x.DVDNumber == data.DVDNumber)
                .Include(x => x.Actor).OrderBy(x => x.Actor.ActorSurname)
                .Select(x => x.Actor.ActorFirstName + " " + x.Actor.ActorSurname).ToList();
            string actors = string.Join(", ", actorList);
            data.actors = actors;
        }

        return View(databaseContext);
    }
    
    //Question 10
    public List<DVDCopy> DVDNotOnLoan()
    {
        List<DVDCopy> dvdCopies = _context.DvdCopies.Include(x => x.DvdTitle).ToList();
        List<DVDCopy> newCopies = dvdCopies.Where(x=>(DateTime.Now.Date - x.DatePurchased.Date).TotalDays >= 365).ToList();
        List<DVDCopy> dvdNotInLoan = new List<DVDCopy>();
        foreach (var copy in newCopies)
        {
            List<Loan> copyLoans = _context.Loans.Where(x=>x.DvdCopy == copy && x.status == "Loaned").ToList();
            if (copyLoans.Count == 0)
            {
                dvdNotInLoan.Add(copy);
            }
        }

        return dvdNotInLoan;
    }

    public IActionResult DVDOlderThan365Days()
    {
        List<DVDCopy> dvdCopies = DVDNotOnLoan();
        return View(dvdCopies);
    }

    public IActionResult deleteDVDOlderThan365Days()
    {
        List<DVDCopy> dvdCopyNotInLoan = DVDNotOnLoan();
        foreach (var dvdCopy in dvdCopyNotInLoan)
        {
            try
            {
                var copy_data = _context.DvdCopies.Where(x=>x.CopyNumber == dvdCopy.CopyNumber).First();
                _context.DvdCopies.Remove(copy_data);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        ViewBag.DeleteMessage = "Copy of DVD has been deleted";
        return Redirect(null);
    }
}
