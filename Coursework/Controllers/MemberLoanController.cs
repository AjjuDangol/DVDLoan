using System.Diagnostics;
using System.Linq;
using Coursework.Data;
using Coursework.Models;
using Coursework.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coursework.Controllers;

public class MemberLoanController : Controller
{
    private readonly ApplicationDbContext _context;

    public MemberLoanController(ApplicationDbContext context)
    {
        _context = context;
    }

    //Question 8
    // public IActionResult MemberLoanDetails()
    // {
    //     List<MemberLoanDetailsDTO> dtos = new List<MemberLoanDetailsDTO>();
    //     List<Member> members = _context.Members.Include(x=>x.MembershipCategory).ToList();
    //     if (members != null)
    //     {
    //         foreach (Member member in members)
    //         {
    //             var membershipCategory = _context.MembershipCategories.Where(x=>x.MembershipCategoryNumber == member.MembershipCategory.MembershipCategoryNumber).First();
    //             int totalLoan = _context.Loans.Include(x=>x.Type == member && x.DateReturned == "Not Returned").ToArray().Length;
    //
    //             if (totalLoan > 0)
    //             {
    //                 MemberLoanDetailsDTO dto = new MemberLoanDetailsDTO();
    //                 dto.address = member.MemberAddress;
    //                 dto.firstName = member.MemberFirstName;
    //                 dto.lastName = member.MemberLastName;
    //                 dto.dateOfBirth = member.MemberDateOfBirth;
    //                 dto.totalLoans = totalLoan;
    //                 dto.description = membershipCategory.MembershipCategoryDescription;
    //                 dtos.Add(dto);
    //             }
    //         }
    //     }
    //
    //     List<MemberLoanDetailsDTO> loanDtos = dtos.OrderBy(x=>x.firstName).ToList();
    //     ViewBag.DTOS = loanDtos;
    //     return View(loanDtos);
    // }
    
    public IActionResult MemberLoanDetails() {
        List<MemberLoanDetailsDTO> dtos = new List<MemberLoanDetailsDTO>();
        List<Member> memberList = _context.Members.Include(x=>x.MembershipCategory).ToList();
        if (memberList != null) {
            foreach (Member member in memberList)
            {
                var membershipCategory = _context.MembershipCategories.Where(x => x.MembershipCategoryNumber == member.MembershipCategory.MembershipCategoryNumber).First();
                int totalLoan= _context.Loans.Include(x=>x.Member).Where(x => x.Member == member 
                    && x.status == "loaned").ToArray().Length;
                // if (totalLoan > membershipCategory.MembershipCategoryTotalLoans) {
                //     remarks = "Too many DVDs";
                // }
                if (totalLoan > 0)
                {
                    MemberLoanDetailsDTO dto = new MemberLoanDetailsDTO();
                    dto.address = member.MemberAddress;
                    dto.firstName = member.MemberFirstName;
                    dto.lastName = member.MemberLastName;
                    dto.dateOfBirth = member.MemberDateOfBirth;
                    dto.totalLoans = totalLoan;
                    dto.description = membershipCategory.MembershipCategoryDescription;
                    dtos.Add(dto);
                }
            }
        }
        List<MemberLoanDetailsDTO> orderedDtos= dtos.OrderBy(x=>x.firstName).ToList();
        ViewBag.DTOS = orderedDtos;
        return View(orderedDtos);
    }
}