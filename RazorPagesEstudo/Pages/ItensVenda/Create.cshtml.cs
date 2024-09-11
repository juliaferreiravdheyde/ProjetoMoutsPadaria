using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorPagesEstudo.Data;
using RazorPagesEstudo.Models;

namespace RazorPagesEstudo.Pages.ItensVenda
{
    public class CreateModel : PageModel
    {
        private readonly RazorPagesEstudo.Data.RazorPagesEstudoContext _context;

        public CreateModel(RazorPagesEstudo.Data.RazorPagesEstudoContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["ProdutoId"] = new SelectList(_context.Produto, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public ItemVenda ItemVenda { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.ItemVenda.Add(ItemVenda);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
