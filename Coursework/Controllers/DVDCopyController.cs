using Coursework.Data;
using Coursework.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Coursework.Models.DTO;

namespace Coursework.Controllers
{
    public class DVDCopyController : Controller
    {
        private ApplicationDbContext _context;

        //public DVDCategoryController(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        public DVDCopyController(ApplicationDbContext context)
        {
            _context = context; 
        }
        public IActionResult getDVDCopyDetails(int copyNumber)
        {
            DVDCopy dvdCopy = _context.DvdCopies.Include(x => x.DvdTitle).Where(x => x.CopyNumber == copyNumber).First();
            Loan loan = new Loan();
            DVDTitle dvd = new DVDTitle();
            if (dvdCopy != null)
            {
                loan = _context.Loans.Include(x => x.DvdCopy).ThenInclude(x => x.DvdTitle).Include(x => x.Member).Where(x => x.DvdCopy == dvdCopy).Last();
            }
            if (dvdCopy != null)
            {
                dvd = _context.DvdTitles.Where(x => x.DVDNumber == dvdCopy.DvdTitle.DVDNumber).Last();
            }
            Member member = new Member();
            if (loan != null)
            {
                member = _context.Members.Where(x => x.MemberNumber == loan.Member.MemberNumber).First();
            }

            DVDCopyDTO detailDTO = new DVDCopyDTO();
            detailDTO.FullName = member.MemberFirstName + " " + member.MemberLastName;
            detailDTO.DvdTitle = dvd.DvdTitle;
            detailDTO.DateOut = loan.DateOut;
            detailDTO.DateDue = loan.DateDue;
            detailDTO.DateReturned = loan.DateReturned;
            ViewBag.DTOs = detailDTO;
            return View(detailDTO);
        }
    }
}
