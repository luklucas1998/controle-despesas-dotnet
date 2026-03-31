using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ControleDespesas.Data;
using ControleDespesas.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ControleDespesas.Controllers
{
    [Authorize]
    public class DespesasController : Controller
    {
        private readonly AppDbContext _context;

        public DespesasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Despesas
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var despesas = await _context.Despesas
                .Where(d => d.UserId == userId)
                .ToListAsync();

            ViewBag.Total = despesas.Sum(x => x.Valor);

            return View(despesas);
        }

        // GET: Despesas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var despesa = await _context.Despesas
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

            if (despesa == null) return NotFound();

            return View(despesa);
        }

        // GET: Despesas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Despesas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Despesa despesa)
        {
            if (ModelState.IsValid)
            {
                despesa.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _context.Add(despesa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(despesa);
        }

        // GET: Despesas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var despesa = await _context.Despesas
                .FirstOrDefaultAsync(d => d.Id == id && d.UserId == userId);

            if (despesa == null) return NotFound();

            return View(despesa);
        }

        // POST: Despesas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Despesa despesa)
        {
            if (id != despesa.Id) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var despesaBanco = await _context.Despesas
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == id && d.UserId == userId);

            if (despesaBanco == null) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    despesa.UserId = userId; // garante que não perde vínculo

                    _context.Update(despesa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DespesaExists(despesa.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(despesa);
        }

        // GET: Despesas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var despesa = await _context.Despesas
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

            if (despesa == null) return NotFound();

            return View(despesa);
        }

        // POST: Despesas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var despesa = await _context.Despesas
                .FirstOrDefaultAsync(d => d.Id == id && d.UserId == userId);

            if (despesa != null)
            {
                _context.Despesas.Remove(despesa);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Dashboard()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var despesas = await _context.Despesas
                .Where(d => d.UserId == userId)
                .ToListAsync(); // 🔥 traz tudo primeiro

            var dados = despesas
                .GroupBy(d => d.Categoria)
                .Select(g => new
                {
                    Categoria = g.Key,
                    Total = g.Sum(x => x.Valor)
                })
                .ToList();

            ViewBag.Total = dados.Sum(x => x.Total);

            return View(dados);
        }

        private bool DespesaExists(int id)
        {
            return _context.Despesas.Any(e => e.Id == id);
        }
    }
}