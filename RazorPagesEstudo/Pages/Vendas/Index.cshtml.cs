using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPagesEstudo.Data;
using RazorPagesEstudo.Models;
using RazorPagesEstudo.Services;
using System;
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
        public List<Cliente> ClientesDisponiveis { get; set; }
        [BindProperty]
        public List<ItemVenda> ItensVenda { get; set; } = new List<ItemVenda>();
        [BindProperty]
        public string FormaPagamento { get; set; }
        [BindProperty]
        public string NomeCliente { get; set; }
        public string cpfCnpj { get; set; }
        public Cliente Cliente { get; set; }
        //public Pessoa Pessoa { get; set; }

        public decimal Total { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            ProdutosDisponiveis = await _context.Produto.ToListAsync();

            ClientesDisponiveis = await _context.Cliente.ToListAsync();
            return Page();
        }
     
        public async Task<IActionResult> OnPostAsync(int[] selectedProducts, int[] quantidades, string formaPagamento, string cpfCnpj)
        {
            
            if (selectedProducts.Length == 0 || string.IsNullOrEmpty(formaPagamento) || selectedProducts.Length != quantidades.Length)
            {
                ModelState.AddModelError(string.Empty, "Selecione produtos e informe a forma de pagamento. As quantidades devem corresponder aos produtos.");
                ProdutosDisponiveis = await _context.Produto.ToListAsync();
                return Page();
            }

            ItensVenda = new List<ItemVenda>();

            for (int i = 0; i < selectedProducts.Length; i++)
            {
                var produtoId = selectedProducts[i];
                var quantidade = quantidades[i];

                var produto = await _context.Produto.FindAsync(produtoId);
                if (produto != null && quantidade > 0)
                {
                    var itemVenda = new ItemVenda(produto, quantidade);
                    ItensVenda.Add(itemVenda);
                }
            }

            Cliente = await _context.Cliente.FirstOrDefaultAsync(c => c.CpfCnpj == cpfCnpj);

            decimal total = ItensVenda.Sum(item => item.ValorTotal);

            var novaVenda = new Models.Venda
            {
                FormaPagamento = formaPagamento,
                ValorTotal = total, 
                Cliente = Cliente,
            };

            try
            {
                _vendaService.AddVenda(novaVenda);
                _vendaService.AtualizarPontosFidelidade(Cliente, ItensVenda);
                return RedirectToPage("Confirmacao", new { vendaId = novaVenda.Id });
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ProdutosDisponiveis = await _context.Produto.ToListAsync();
                return Page();
            }
        }
    }
}