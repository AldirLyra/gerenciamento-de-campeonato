using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace gerenciamento_de_campeonato.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("Campeonato") { }
        public DbSet<Time> Times { get; set; }
        public DbSet<Jogador> Jogadores { get; set; }
        public DbSet<ComissaoTecnica> ComissaoTecnica { get; set; }
        public DbSet<Partida> Partidas { get; set; }
        public DbSet<Tabela> Tabelas { get; set; }
        public DbSet<Liga> Liga { get; set; }
        public DbSet<Gol> Gol { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Time>()
                .HasMany(t => t.Jogadores)
                .WithRequired(j => j.Time)
                .HasForeignKey(j => j.TimeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Time>()
                .HasMany(t => t.ComissaoTecnicas)
                .WithRequired(ct => ct.Time)
                .HasForeignKey(ct => ct.TimeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Time>()
                .HasRequired(t => t.Liga)
                .WithMany(l => l.Times)
                .HasForeignKey(t => t.LigaId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Partida>()
                .HasRequired(p => p.TimeCasa)
                .WithMany()
                .HasForeignKey(p => p.TimeCasaId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Partida>()
                .HasRequired(p => p.TimeVisitante)
                .WithMany()
                .HasForeignKey(p => p.TimeVisitanteId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Partida>()
                .HasRequired(p => p.Liga)
                .WithMany(l => l.Partidas)
                .HasForeignKey(p => p.LigaId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Gol>()
                .HasRequired(g => g.Partida)
                .WithMany(p => p.Gols)
                .HasForeignKey(g => g.PartidaId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Gol>()
                .HasRequired(g => g.Jogador)
                .WithMany(j => j.Gols)
                .HasForeignKey(g => g.JogadorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tabela>()
                .HasRequired(t => t.Time)
                .WithMany()
                .HasForeignKey(t => t.TimeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tabela>()
                .HasRequired(t => t.Liga)
                .WithMany(l => l.Tabela)
                .HasForeignKey(t => t.LigaId)
                .WillCascadeOnDelete(false);
        }
    }
}