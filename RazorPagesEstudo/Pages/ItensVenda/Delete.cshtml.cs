using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPagesEstudo.Data;
using RazorPagesEstudo.Models;

namespace RazorPagesEstudo.Pages.ItensVenda
{
    public class DeleteModel : PageModel
    {
        private readonly RazorPagesEstudo.Data.RazorPagesEstudoContext _context;

        public DeleteModel(RazorPagesEstudo.Data.RazorPagesEstudoContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ItemVenda ItemVenda { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemvenda = await _context.ItemVenda.FirstOrDefaultAsync(m => m.Id == id);

            if (itemvenda == null)
            {
                return NotFound();
            }
            else
            {
                ItemVenda = itemvenda;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemvenda = await _context.ItemVenda.FindAsync(id);
            if (itemvenda != null)
            {
                ItemVenda = itemvenda;
                _context.ItemVenda.Remove(ItemVenda);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
