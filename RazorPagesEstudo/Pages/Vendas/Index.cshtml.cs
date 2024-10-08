﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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
        private readonly ProdutoService _produtoService;
        private readonly ClienteService _clienteService;
        private readonly RazorPagesEstudo.Data.RazorPagesEstudoContext _context;

        public IndexModel(VendaService vendaService, ClienteService clienteService, RazorPagesEstudo.Data.RazorPagesEstudoContext context)
        {
            _vendaService = vendaService;
            _clienteService = clienteService;
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
        public string CpfCnpj { get; set; }
        public Cliente Cliente { get; set; }

        public decimal Total { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Modelo usando Service para estudo 
            //ProdutosDisponiveis = await _produtoService.GetAvailableProductsAsync();

            // Modelo usando Linq para estudo 
            ProdutosDisponiveis = await _context.Produto
                                    .Where(produto => produto.QuantidadeEstoque > 0)
                                    .ToListAsync();

            return Page();
        }

        // Modelo usando Linq de Produtos disponiveis para estudo 
        public async Task<IActionResult> OnPostAsync(int[] selectedProducts, int[] quantidades, string formaPagamento, string cpfCnpj)
        {
            ItensVenda = new List<ItemVenda>();

            for (int i = 0; i < selectedProducts.Length; i++)
            {
                var produtoId = selectedProducts[i];

                var produto = await _context.Produto.FirstOrDefaultAsync(p => p.Id == produtoId);
                if (produto != null && quantidades[i] > 0)
                {
                    var itemVenda = new ItemVenda(produto, quantidades[i]);
                    ItensVenda.Add(itemVenda);
                }
            }

            if (cpfCnpj != null)
            {
                try
                {
                    Cliente = await _clienteService.GetClienteByCpfCnpjAsync(cpfCnpj);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Cliente não encontrado: " + ex.Message);
                    return Page();
                }
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
                var vendaCriada = await _vendaService.AddVendaAsync(novaVenda);
                _vendaService.AddItensVenda(ItensVenda, vendaCriada.Id);

                _vendaService.AtualizarPontosFidelidade(Cliente, ItensVenda);

                return RedirectToPage("Confirmacao", new { vendaId = vendaCriada.Id });
            }
            catch (Exception ex)
            {
                ProdutosDisponiveis = await _context.Produto
                                        .Where(produto => produto.QuantidadeEstoque > 0)
                                        .ToListAsync();

                ModelState.AddModelError(string.Empty, "Erro ao registrar a venda: " + ex.Message);
                return Page();
            }
        }
        // modelo usando Service para buscar produtos disponiveis para estudo 
        /*
                public async Task<IActionResult> OnPostAsync(int[] selectedProducts, int[] quantidades, string formaPagamento, string cpfCnpj)
                {
                    ItensVenda = new List<ItemVenda>();

                    for (int i = 0; i < selectedProducts.Length; i++)
                    {
                        var produtoId = selectedProducts[i];

                        var produto = await _produtoService.GetProdutoByIdAsync(produtoId);
                        if (produto != null && quantidades[i] > 0)
                        {
                            var itemVenda = new ItemVenda(produto, quantidades[i]);
                            ItensVenda.Add(itemVenda);
                        }
                    }

                    if (cpfCnpj != null) 
                    {
                        try
                        {
                            Cliente = await _clienteService.GetClienteByCpfCnpjAsync(cpfCnpj);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError(string.Empty, "Cliente não encontrado: " + ex.Message);
                            return Page();
                        }
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
                        var vendaCriada = await _vendaService.AddVendaAsync(novaVenda); 
                        _vendaService.AddItensVenda(ItensVenda, vendaCriada.Id);

                        _vendaService.AtualizarPontosFidelidade(Cliente, ItensVenda);

                        return RedirectToPage("Confirmacao", new { vendaId = vendaCriada.Id });
                    }
                    catch (Exception ex)
                    {
                        ProdutosDisponiveis = await _produtoService.GetAvailableProductsAsync();
                        ModelState.AddModelError(string.Empty, "Erro ao registrar a venda: " + ex.Message);
                        return Page();
                    }
                }
                */
    }
}
