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
    public class IndexModel : PageModel
    {
        private readonly RazorPagesEstudo.Data.RazorPagesEstudoContext _context;

        public IndexModel(RazorPagesEstudo.Data.RazorPagesEstudoContext context)
        {
            _context = context;
        }

        public IList<Venda> Venda { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Venda = await _context.Venda
                .Include(v => v.Cliente).ToListAsync();
        }
    }
}
