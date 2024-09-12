using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPagesEstudo.Data;
using RazorPagesEstudo.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RazorPagesEstudo.Pages.Vendas
{
    public class IndexModel : PageModel
    {
        private readonly RazorPagesEstudoContext _context;

        public IndexModel(RazorPagesEstudoContext context)
        {
            _context = context;
        }

        [BindProperty]
        public List<Produto> ProdutosDisponiveis { get; set; }
        [BindProperty]
        public List<ItemVenda> ItensVenda { get; set; } = new List<ItemVenda>();
        [BindProperty]
        public string FormaPagamento { get; set; }
        [BindProperty]
        public string NomeCliente { get; set; }
        public Cliente Cliente { get; set; }

        public decimal Total { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            ProdutosDisponiveis = await _context.Produto.ToListAsync(); // Access the correct entity set for Produto
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int[] selectedProducts, string formaPagamento, string nomeCliente)
        {
            if (selectedProducts.Length == 0 || string.IsNullOrEmpty(formaPagamento))
            {
                ModelState.AddModelError(string.Empty, "Selecione produtos e informe a forma de pagamento.");
                ProdutosDisponiveis = await _context.Produto.ToListAsync();
                return Page();
            }

            foreach (var produtoId in selectedProducts)
            {
                var produto = await _context.Produto.FindAsync(produtoId); // Access the correct entity set for Produto
                if (produto != null)
                {
                    var itemVenda = new ItemVenda(produto, 1); // Assuming quantity is 1 for now
                    ItensVenda.Add(itemVenda);
                }
            }

            Cliente = await _context.Cliente.FirstOrDefaultAsync(c => c.Nome == nomeCliente); // Access the correct entity set for Cliente

            var venda = new Venda(ItensVenda, formaPagamento, Cliente);
            Total = venda.Total;

            // Save sale to the database
            var novaVenda = new Models.Venda
            {
                FormaPagamento = formaPagamento,
                Cliente = Cliente,
                ItensVenda = ItensVenda
            };
            _context.Venda.Add(novaVenda); // Access the correct entity set for Venda
            await _context.SaveChangesAsync();

            return RedirectToPage("Confirmacao", new { vendaId = novaVenda.Id });
        }
    }
}
