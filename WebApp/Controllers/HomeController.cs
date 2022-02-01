using WebApp.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApp.Models;
using Microsoft.Extensions.Configuration;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var tickets = await _context.TravelTicket.ToListAsync();
            return View(tickets);
        }

        public async Task<IActionResult> AddOrEdit(int? Id)
        {
            ViewBag.PageName = Id == null ? "Create Ticket" : "Edit Ticket";
            ViewBag.IsEdit = Id == null ? false : true;
            if (Id == null)
            {
                return View();
            }
            else
            {
                var ticket = await _context.TravelTicket.FindAsync(Id);

                if ( ticket == null)
                {
                    return NotFound();
                }
                return View(ticket);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int Id, [Bind("Id,Origin,Destination,Cost")]
        TravelTicket travelticketData)
        {
            bool IsTicketExist = false;

            TravelTicket ticket = await _context.TravelTicket.FindAsync(Id);

            if (ticket != null)
            {
                IsTicketExist = true;
            }
            else
            {
                ticket = new TravelTicket();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ticket.Id = travelticketData.Id;
                    ticket.Origin = travelticketData.Origin;
                    ticket.Destination = travelticketData.Destination;
                    ticket.Cost = travelticketData.Cost;

                    if (IsTicketExist)
                    {
                        _context.Update(ticket);
                    }
                    else
                    {
                        _context.Add(ticket);
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(travelticketData);
        }

        public async Task<IActionResult> Details(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var travelTicket = await _context.TravelTicket.FirstOrDefaultAsync(ticket => ticket.Id == Id);
            if (travelTicket == null)
            {
                return NotFound();
            }
            return View(travelTicket);
        }

        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var travelTicket = await _context.TravelTicket.FirstOrDefaultAsync(ticket => ticket.Id == Id);

            return View(travelTicket);
        }

        // POST: Employees/Delete/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int Id)
        {
            var travelTicket = await _context.TravelTicket.FindAsync(Id);
            _context.TravelTicket.Remove(travelTicket);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
                       {
                            return View();
                        }

                    public IActionResult settings()
                        {
                            return View();
                        }

                    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
                    public IActionResult Error()
                        {
                            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                        }
                    }
                }
