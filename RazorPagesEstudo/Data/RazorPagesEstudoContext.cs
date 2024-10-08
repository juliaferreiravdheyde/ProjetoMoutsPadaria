﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RazorPagesEstudo.Models;

namespace RazorPagesEstudo.Data
{
    public class RazorPagesEstudoContext : DbContext
    {
        public RazorPagesEstudoContext (DbContextOptions<RazorPagesEstudoContext> options)
            : base(options)
        {
        }

        public DbSet<RazorPagesEstudo.Models.Venda> Venda { get; set; } = default!;
        public DbSet<RazorPagesEstudo.Models.Produto> Produto { get; set; } = default!;
        public DbSet<RazorPagesEstudo.Models.ItemVenda> ItemVenda { get; set; } = default!;
        public DbSet<RazorPagesEstudo.Models.Cliente> Cliente { get; set; } = default!;
        public IEnumerable<object> Pessoa { get; internal set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pessoa>()
                .ToTable("Pessoa");

            modelBuilder.Entity<Cliente>()
                .ToTable("Cliente")
                .HasBaseType<Pessoa>();
        }
    }
}
