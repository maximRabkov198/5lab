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
    public class CarsController : Controller
    {
        private readonly taxiContext _context;

        public CarsController(taxiContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "admin,user")]
        public IActionResult Index(string Name, int? Experiance, int Age, string Machanic, int RegistrationNumber, int BodyNumber, int PhoneNumber, string DstinationNumber, int page = 1, SortState sortOrder = SortState.AgeAcs)
        {
            int pageSize = 10;

            IQueryable<Cars> source = _context.Cars.Include(c => c.Driver).Include(c => c.Mark);

            ViewData["RegistrationNumberSort"] = sortOrder == SortState.RegistrationNumberAcs ? SortState.RegistrationNumberDesc : SortState.RegistrationNumberAcs;
            ViewData["BodyNumberSort"] = sortOrder == SortState.BodyNumberAcs ? SortState.BodyNumberDesc : SortState.BodyNumberAcs;
            ViewData["MachanicSort"] = sortOrder == SortState.MachanicAcs ? SortState.MachanicDesc : SortState.MachanicAcs;

            if (RegistrationNumber != null)
            {
                source = source.Where(x => x.RegistrationNumber == RegistrationNumber);
            }
            if (BodyNumber != null)
            {
                source = source.Where(x => x.BodyNumber == BodyNumber);
            }
            if (Machanic != null)
            {
                source = source.Where(x => x.Machanic == Machanic);
            }

            switch (sortOrder)
            {
                case SortState.RegistrationNumberAcs:
                    source = source.OrderBy(x => x.RegistrationNumber);
                    break;
                case SortState.RegistrationNumberDesc:
                    source = source.OrderByDescending(x => x.RegistrationNumber);
                    break;
                case SortState.BodyNumberAcs:
                    source = source.OrderBy(x => x.BodyNumber);
                    break;
                case SortState.BodyNumberDesc:
                    source = source.OrderByDescending(x => x.BodyNumber);
                    break;
                case SortState.MachanicAcs:
                    source = source.OrderBy(x => x.Machanic);
                    break;
                case SortState.MachanicDesc:
                    source = source.OrderByDescending(x => x.Machanic);
                    break;
            }


            var count = source.Count();
            var items = source.Skip((page - 1) * pageSize).Take(pageSize);
            PageViewModel pageView = new PageViewModel(count, page, pageSize);

            IndexViewModel ivm = new IndexViewModel
            {
                PageViewModel = pageView,
                FilterView = new FilterView(Name,Experiance,Age,Machanic,RegistrationNumber,BodyNumber,PhoneNumber,DstinationNumber),
                Cars= items
            };
            return View(ivm);

        }
        [Authorize(Roles = "admin,user")]
        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cars = await _context.Cars
                .Include(c => c.Driver)
                .Include(c => c.Mark)
                .SingleOrDefaultAsync(m => m.Carsid == id);
            if (cars == null)
            {
                return NotFound();
            }

            return View(cars);
        }
        [Authorize(Roles = "admin")]
        // GET: Cars/Create
        public IActionResult Create()
        {
            ViewData["DriverId"] = new SelectList(_context.Associates, "Associatesid", "Associatesid");
            ViewData["MarkId"] = new SelectList(_context.CarsMark, "CarsMarkid", "CarsMarkid");
            return View();
        }
        [Authorize(Roles = "admin")]
        // POST: Cars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Carsid,RegistrationNumber,MarkId,BodyNumber,Data,EngineNumber,Year,Mileage,DateOfLastMaintetance,Machanic,SpecialMarks,DriverId")] Cars cars)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cars);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DriverId"] = new SelectList(_context.Associates, "Associatesid", "Associatesid", cars.DriverId);
            ViewData["MarkId"] = new SelectList(_context.CarsMark, "CarsMarkid", "CarsMarkid", cars.MarkId);
            return View(cars);
        }
        [Authorize(Roles = "admin")]
        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cars = await _context.Cars.SingleOrDefaultAsync(m => m.Carsid == id);
            if (cars == null)
            {
                return NotFound();
            }
            ViewData["DriverId"] = new SelectList(_context.Associates, "Associatesid", "Associatesid", cars.DriverId);
            ViewData["MarkId"] = new SelectList(_context.CarsMark, "CarsMarkid", "CarsMarkid", cars.MarkId);
            return View(cars);
        }
        [Authorize(Roles = "admin")]
        // POST: Cars/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Carsid,RegistrationNumber,MarkId,BodyNumber,Data,EngineNumber,Year,Mileage,DateOfLastMaintetance,Machanic,SpecialMarks,DriverId")] Cars cars)
        {
            if (id != cars.Carsid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cars);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarsExists(cars.Carsid))
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
            ViewData["DriverId"] = new SelectList(_context.Associates, "Associatesid", "Associatesid", cars.DriverId);
            ViewData["MarkId"] = new SelectList(_context.CarsMark, "CarsMarkid", "CarsMarkid", cars.MarkId);
            return View(cars);
        }
        [Authorize(Roles = "admin")]
        // GET: Cars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cars = await _context.Cars
                .Include(c => c.Driver)
                .Include(c => c.Mark)
                .SingleOrDefaultAsync(m => m.Carsid == id);
            if (cars == null)
            {
                return NotFound();
            }

            return View(cars);
        }
        [Authorize(Roles = "admin")]
        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cars = await _context.Cars.SingleOrDefaultAsync(m => m.Carsid == id);
            _context.Cars.Remove(cars);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarsExists(int id)
        {
            return _context.Cars.Any(e => e.Carsid == id);
        }
    }
}
