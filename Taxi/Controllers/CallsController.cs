using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Taxi.Models;
using Taxi.ViewModelss;
using Taxi.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Taxi.Controllers
{
    public class CallsController : Controller
    {
        private readonly taxiContext _context;

        public CallsController(taxiContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "admin,user")]
        public IActionResult Index(string Name, int? Experiance, int Age, string Machanic, int RegistrationNumber, int BodyNumber, int PhoneNumber, string DstinationNumber, int page = 1, SortState sortOrder = SortState.AgeAcs)
        {
            int pageSize = 10;

            IQueryable<Calls> source = _context.Calls.Include(c => c.Car);

            ViewData["PhoneNumberSort"] = sortOrder == SortState.PhoneNumberAcs ? SortState.PhoneNumberDesc : SortState.PhoneNumberAcs;
            ViewData["DstinationNumberSort"] = sortOrder == SortState.DstinationNumberAcs? SortState.DstinationNumberDesc : SortState.DstinationNumberAcs;


            if (PhoneNumber != null)
            {
                source = source.Where(x => x.PhoneNumber == PhoneNumber);
            }
            if (DstinationNumber != null)
            {
                source = source.Where(x => x.DstinationNumber == DstinationNumber);
            }


            switch (sortOrder)
            {
                case SortState.PhoneNumberAcs:
                    source = source.OrderBy(x => x.PhoneNumber);
                    break;
                case SortState.PhoneNumberDesc:
                    source = source.OrderByDescending(x => x.PhoneNumber);
                    break;
                case SortState.DstinationNumberAcs:
                    source = source.OrderBy(x => x.DstinationNumber);
                    break;
                case SortState.DstinationNumberDesc:
                    source = source.OrderByDescending(x => x.DstinationNumber);
                    break;
            }


            var count = source.Count();
            var items = source.Skip((page - 1) * pageSize).Take(pageSize);
            PageViewModel pageView = new PageViewModel(count, page, pageSize);

            IndexViewModel ivm = new IndexViewModel
            {
                PageViewModel = pageView,
                FilterView = new FilterView(Name, Experiance, Age, Machanic, RegistrationNumber, BodyNumber, PhoneNumber, DstinationNumber),
                Calls = items
            };
            return View(ivm);

        }
        [Authorize(Roles = "admin,user")]
        // GET: Calls/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calls = await _context.Calls
                .Include(c => c.Car)
                .SingleOrDefaultAsync(m => m.Callsid == id);
            if (calls == null)
            {
                return NotFound();
            }

            return View(calls);
        }
        [Authorize(Roles = "admin")]
        // GET: Calls/Create
        public IActionResult Create()
        {
            ViewData["CarId"] = new SelectList(_context.Cars, "Carsid", "Carsid");
            return View();
        }
        [Authorize(Roles = "admin")]
        // POST: Calls/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Callsid,DateAndTime,PhoneNumber,DstinationNumber,CarId")] Calls calls)
        {
            if (ModelState.IsValid)
            {
                _context.Add(calls);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarId"] = new SelectList(_context.Cars, "Carsid", "Carsid", calls.CarId);
            return View(calls);
        }
        [Authorize(Roles = "admin")]
        // GET: Calls/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calls = await _context.Calls.SingleOrDefaultAsync(m => m.Callsid == id);
            if (calls == null)
            {
                return NotFound();
            }
            ViewData["CarId"] = new SelectList(_context.Cars, "Carsid", "Carsid", calls.CarId);
            return View(calls);
        }
        [Authorize(Roles = "admin")]
        // POST: Calls/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Callsid,DateAndTime,PhoneNumber,DstinationNumber,CarId")] Calls calls)
        {
            if (id != calls.Callsid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(calls);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CallsExists(calls.Callsid))
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
            ViewData["CarId"] = new SelectList(_context.Cars, "Carsid", "Carsid", calls.CarId);
            return View(calls);
        }
        [Authorize(Roles = "admin")]
        // GET: Calls/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var calls = await _context.Calls
                .Include(c => c.Car)
                .SingleOrDefaultAsync(m => m.Callsid == id);
            if (calls == null)
            {
                return NotFound();
            }

            return View(calls);
        }
        [Authorize(Roles = "admin")]
        // POST: Calls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var calls = await _context.Calls.SingleOrDefaultAsync(m => m.Callsid == id);
            _context.Calls.Remove(calls);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CallsExists(int id)
        {
            return _context.Calls.Any(e => e.Callsid == id);
        }
    }
}
