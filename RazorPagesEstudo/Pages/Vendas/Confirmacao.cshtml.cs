using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPagesEstudo.Data;
using RazorPagesEstudo.Models;
using System.Threading.Tasks;

namespace RazorPagesEstudo.Pages.Vendas
{
    public class ConfirmacaoModel : PageModel
    {
        private readonly RazorPagesEstudoContext _context;

        public ConfirmacaoModel(RazorPagesEstudoContext context)
        {
            _context = context;
        }

        public Venda Venda { get; set; }

        public async Task<IActionResult> OnGetAsync(int vendaId)
        {
            Venda = await _context.Venda
                .Include(v => v.Cliente)
                .Include(v => v.ItensVenda)
                    .ThenInclude(iv => iv.Produto)
                .FirstOrDefaultAsync(v => v.Id == vendaId);

            if (Venda == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
