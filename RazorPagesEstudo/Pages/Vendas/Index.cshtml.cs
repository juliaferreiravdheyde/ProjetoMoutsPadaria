using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        private readonly VendaService _vendaService;

        public IndexModel(VendaService vendaService)
        {
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
        public string CpfCnpj { get; set; }
        public Cliente Cliente { get; set; }

        public decimal Total { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            ProdutosDisponiveis = await _vendaService.GetAvailableProductsAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int[] selectedProducts, int[] quantidades, string formaPagamento, string cpfCnpj)
        {
            ItensVenda = new List<ItemVenda>();

            for (int i = 0; i < selectedProducts.Length; i++)
            {
                var produtoId = selectedProducts[i];

                var produto = await _vendaService.GetProdutoByIdAsync(produtoId);
                if (produto != null && quantidades[i] > 0)
                {
                    var itemVenda = new ItemVenda(produto, quantidades[i]);
                    ItensVenda.Add(itemVenda);
                }
            }
            try
            {
                Cliente = await _vendaService.GetClienteByCpfCnpjAsync(cpfCnpj);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Cliente nao encontrado " + ex.Message);
            }

            decimal total = ItensVenda.Sum(item => item.ValorTotal);

            var novaVenda = new Models.Venda
            {
                FormaPagamento = formaPagamento,
                ValorTotal = total,
                Cliente = Cliente,
                ItensVenda = ItensVenda
            };

            try
            {
                var vendaCriada = _vendaService.AddVenda(novaVenda);
                _vendaService.AddItensVenda(ItensVenda, vendaCriada.Id);

                _vendaService.AtualizarPontosFidelidade(Cliente, ItensVenda);

                return RedirectToPage("Confirmacao", new { vendaId = vendaCriada.Id });
            }
            catch (Exception ex)
            {
                ProdutosDisponiveis = await _vendaService.GetAvailableProductsAsync();  
                ModelState.AddModelError(string.Empty, "Erro ao registrar a venda: " + ex.Message);
                return Page();
            }
        }
    }
}
