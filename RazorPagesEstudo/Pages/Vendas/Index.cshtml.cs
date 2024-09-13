using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPagesEstudo.Data;
using RazorPagesEstudo.Models;
using RazorPagesEstudo.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RazorPagesEstudo.Pages.Vendas
{
    public class IndexModel : PageModel
    {
        private readonly RazorPagesEstudoContext _context;
        private readonly VendaService _vendaService;

        public IndexModel(RazorPagesEstudoContext context, VendaService vendaService)
        {
            _context = context;
            _vendaService = vendaService;
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
            ProdutosDisponiveis = await _context.Produto.ToListAsync();
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
                var produto = await _context.Produto.FindAsync(produtoId);
                if (produto != null)
                {
                    var itemVenda = new ItemVenda(produto, 1);
                    ItensVenda.Add(itemVenda);
                }
            }

            Cliente = await _context.Cliente.FirstOrDefaultAsync(c => c.Nome == nomeCliente);

            var venda = new Venda(ItensVenda, formaPagamento, Cliente);
            Total = venda.Total;

            // Save the sale to the database
            var novaVenda = new Models.Venda
            {
                FormaPagamento = formaPagamento,
                Cliente = Cliente,
                ItensVenda = ItensVenda
            };
            _context.Venda.Add(novaVenda);
            await _context.SaveChangesAsync();

            // Update customer loyalty points
            _vendaService.AtualizarPontosFidelidade(Cliente, ItensVenda);

            return RedirectToPage("Confirmacao", new { vendaId = novaVenda.Id });
        }
    }
}
