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
    public class AssociatesController : Controller
    {
        private readonly taxiContext _context;

        public AssociatesController(taxiContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "admin,user")]

        public IActionResult Index(string Name, int? Experiance, int Age, string Machanic, int RegistrationNumber, int BodyNumber, int PhoneNumber, string DstinationNumber, int page = 1, SortState sortOrder = SortState.AgeAcs)
        {
            int pageSize = 5;

            IQueryable<Associates> source = _context.Associates.Include(a => a.Post);

            ViewData["ExperianceSort"] = sortOrder == SortState.ExperianceAcs ? SortState.ExperianceDesc : SortState.ExperianceAcs;
            ViewData["AgeSort"] = sortOrder == SortState.AgeAcs ? SortState.AgeDesc : SortState.AgeAcs;
            ViewData["NameSort"] = sortOrder == SortState.NameAcs ? SortState.NameDesc : SortState.NameAcs;

         

            switch (sortOrder)
            {
                case SortState.ExperianceAcs:
                    source = source.OrderBy(x => x.Experiance);
                    break;
                case SortState.ExperianceDesc:
                    source = source.OrderByDescending(x => x.Experiance);
                    break;
                case SortState.AgeAcs:
                    source = source.OrderBy(x => x.Age);
                    break;
                case SortState.AgeDesc:
                    source = source.OrderByDescending(x => x.Age);
                    break;
                case SortState.NameAcs:
                    source = source.OrderBy(x => x.Name);
                    break;
                case SortState.NameDesc:
                    source = source.OrderByDescending(x => x.Name);
                    break;
            }


            var count = source.Count();
            var items = source.Skip((page - 1) * pageSize).Take(pageSize);
            PageViewModel pageView = new PageViewModel(count, page, pageSize);

            IndexViewModel ivm = new IndexViewModel
            {
                PageViewModel = pageView,
                FilterView = new FilterView(Name, Experiance, Age, Machanic, RegistrationNumber, BodyNumber, PhoneNumber, DstinationNumber),
                Associates = items
            };
            return View(ivm);

        }


        [Authorize(Roles = "admin,user")]

        // GET: Associates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var associates = await _context.Associates
                .Include(a => a.Post)
                .SingleOrDefaultAsync(m => m.Associatesid == id);
            if (associates == null)
            {
                return NotFound();
            }

            return View(associates);
        }
        [Authorize(Roles = "admin")]
        // GET: Associates/Create
        public IActionResult Create()
        {
            ViewData["PostId"] = new SelectList(_context.Post, "Postid", "Postid");
            return View();
        }
        [Authorize(Roles = "admin")]
        // POST: Associates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Associatesid,Experiance,Age,Name,PostId")] Associates associates)
        {
            if (ModelState.IsValid)
            {
                _context.Add(associates);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PostId"] = new SelectList(_context.Post, "Postid", "Postid", associates.PostId);
            return View(associates);
        }
        [Authorize(Roles = "admin")]
        // GET: Associates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var associates = await _context.Associates.SingleOrDefaultAsync(m => m.Associatesid == id);
            if (associates == null)
            {
                return NotFound();
            }
            ViewData["PostId"] = new SelectList(_context.Post, "Postid", "Postid", associates.PostId);
            return View(associates);
        }
        [Authorize(Roles = "admin")]
        // POST: Associates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Associatesid,Experiance,Age,Name,PostId")] Associates associates)
        {
            if (id != associates.Associatesid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(associates);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssociatesExists(associates.Associatesid))
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
            ViewData["PostId"] = new SelectList(_context.Post, "Postid", "Postid", associates.PostId);
            return View(associates);
        }
        [Authorize(Roles = "admin")]
        // GET: Associates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var associates = await _context.Associates
                .Include(a => a.Post)
                .SingleOrDefaultAsync(m => m.Associatesid == id);
            if (associates == null)
            {
                return NotFound();
            }

            return View(associates);
        }
        [Authorize(Roles = "admin")]
        // POST: Associates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var associates = await _context.Associates.SingleOrDefaultAsync(m => m.Associatesid == id);
            _context.Associates.Remove(associates);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssociatesExists(int id)
        {
            return _context.Associates.Any(e => e.Associatesid == id);
        }
    }
}
