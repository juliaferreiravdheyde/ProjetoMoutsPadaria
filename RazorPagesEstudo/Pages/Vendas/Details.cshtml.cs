using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPagesEstudo.Data;
using RazorPagesEstudo.Models;

namespace RazorPagesEstudo.Pages.Vendas
{
    public class DetailsModel : PageModel
    {
        private readonly RazorPagesEstudo.Data.RazorPagesEstudoContext _context;

        public DetailsModel(RazorPagesEstudo.Data.RazorPagesEstudoContext context)
        {
            _context = context;
        }

        public Venda Venda { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Venda.FirstOrDefaultAsync(m => m.Id == id);
            if (venda == null)
            {
                return NotFound();
            }
            else
            {
                Venda = venda;
            }
            return Page();
        }
    }
}
