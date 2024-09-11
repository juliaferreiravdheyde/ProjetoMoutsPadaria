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
    public class IndexModel : PageModel
    {
        private readonly RazorPagesEstudo.Data.RazorPagesEstudoContext _context;

        public IndexModel(RazorPagesEstudo.Data.RazorPagesEstudoContext context)
        {
            _context = context;
        }

        public IList<ItemVenda> ItemVenda { get;set; } = default!;

        public async Task OnGetAsync()
        {
            ItemVenda = await _context.ItemVenda
                .Include(i => i.Produto).ToListAsync();
        }
    }
}
