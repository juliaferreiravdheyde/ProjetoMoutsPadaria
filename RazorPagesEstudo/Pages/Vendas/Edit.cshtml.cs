using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RazorPagesEstudo.Data;
using RazorPagesEstudo.Models;

namespace RazorPagesEstudo.Pages.Vendas
{
    public class EditModel : PageModel
    {
        private readonly RazorPagesEstudo.Data.RazorPagesEstudoContext _context;

        public EditModel(RazorPagesEstudo.Data.RazorPagesEstudoContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Venda Venda { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda =  await _context.Venda.FirstOrDefaultAsync(m => m.Id == id);
            if (venda == null)
            {
                return NotFound();
            }
            Venda = venda;
           ViewData["ClienteID"] = new SelectList(_context.Cliente, "Id", "Id");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Venda).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VendaExists(Venda.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool VendaExists(int id)
        {
            return _context.Venda.Any(e => e.Id == id);
        }
    }
}
