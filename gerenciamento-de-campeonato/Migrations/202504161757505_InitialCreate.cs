namespace gerenciamento_de_campeonato.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ComissaoTecnica",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false),
                        Cargo = c.Int(nullable: false),
                        DataNascimento = c.DateTime(nullable: false),
                        TimeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Time", t => t.TimeId)
                .Index(t => t.TimeId);
            
            CreateTable(
                "dbo.Time",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false),
                        Cidade = c.String(),
                        Estado = c.String(),
                        AnoFundacao = c.Int(nullable: false),
                        Estadio = c.String(),
                        CapacidadeEstadio = c.Int(nullable: false),
                        CorPrimaria = c.String(),
                        CorSecundaria = c.String(),
                        Ativo = c.Boolean(nullable: false),
                        LigaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Liga", t => t.LigaId)
                .Index(t => t.LigaId);
            
            CreateTable(
                "dbo.Jogador",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false),
                        DataNascimento = c.DateTime(nullable: false),
                        Nacionalidade = c.String(),
                        Posicao = c.Int(nullable: false),
                        NumeroCamisa = c.Int(nullable: false),
                        Altura = c.Double(nullable: false),
                        Peso = c.Double(nullable: false),
                        PePreferido = c.Int(nullable: false),
                        TimeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Time", t => t.TimeId)
                .Index(t => t.TimeId);
            
            CreateTable(
                "dbo.Gol",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PartidaId = c.Int(nullable: false),
                        JogadorId = c.Int(nullable: false),
                        Minuto = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Jogador", t => t.JogadorId)
                .ForeignKey("dbo.Partida", t => t.PartidaId)
                .Index(t => t.PartidaId)
                .Index(t => t.JogadorId);
            
            CreateTable(
                "dbo.Partida",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TimeCasaId = c.Int(nullable: false),
                        TimeVisitanteId = c.Int(nullable: false),
                        DataPartida = c.DateTime(nullable: false),
                        GolsTimeCasa = c.Int(nullable: false),
                        GolsTimeVisitante = c.Int(nullable: false),
                        Rodada = c.Int(nullable: false),
                        LigaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Liga", t => t.LigaId)
                .ForeignKey("dbo.Time", t => t.TimeCasaId)
                .ForeignKey("dbo.Time", t => t.TimeVisitanteId)
                .Index(t => t.TimeCasaId)
                .Index(t => t.TimeVisitanteId)
                .Index(t => t.LigaId);
            
            CreateTable(
                "dbo.Liga",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                        Ativa = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tabela",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TimeId = c.Int(nullable: false),
                        LigaId = c.Int(nullable: false),
                        Pontos = c.Int(nullable: false),
                        Jogos = c.Int(nullable: false),
                        Vitorias = c.Int(nullable: false),
                        Empates = c.Int(nullable: false),
                        Derrotas = c.Int(nullable: false),
                        GolsPro = c.Int(nullable: false),
                        GolsContra = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Liga", t => t.LigaId)
                .ForeignKey("dbo.Time", t => t.TimeId)
                .Index(t => t.TimeId)
                .Index(t => t.LigaId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Time", "LigaId", "dbo.Liga");
            DropForeignKey("dbo.Jogador", "TimeId", "dbo.Time");
            DropForeignKey("dbo.Gol", "PartidaId", "dbo.Partida");
            DropForeignKey("dbo.Partida", "TimeVisitanteId", "dbo.Time");
            DropForeignKey("dbo.Partida", "TimeCasaId", "dbo.Time");
            DropForeignKey("dbo.Partida", "LigaId", "dbo.Liga");
            DropForeignKey("dbo.Tabela", "TimeId", "dbo.Time");
            DropForeignKey("dbo.Tabela", "LigaId", "dbo.Liga");
            DropForeignKey("dbo.Gol", "JogadorId", "dbo.Jogador");
            DropForeignKey("dbo.ComissaoTecnica", "TimeId", "dbo.Time");
            DropIndex("dbo.Tabela", new[] { "LigaId" });
            DropIndex("dbo.Tabela", new[] { "TimeId" });
            DropIndex("dbo.Partida", new[] { "LigaId" });
            DropIndex("dbo.Partida", new[] { "TimeVisitanteId" });
            DropIndex("dbo.Partida", new[] { "TimeCasaId" });
            DropIndex("dbo.Gol", new[] { "JogadorId" });
            DropIndex("dbo.Gol", new[] { "PartidaId" });
            DropIndex("dbo.Jogador", new[] { "TimeId" });
            DropIndex("dbo.Time", new[] { "LigaId" });
            DropIndex("dbo.ComissaoTecnica", new[] { "TimeId" });
            DropTable("dbo.Tabela");
            DropTable("dbo.Liga");
            DropTable("dbo.Partida");
            DropTable("dbo.Gol");
            DropTable("dbo.Jogador");
            DropTable("dbo.Time");
            DropTable("dbo.ComissaoTecnica");
        }
    }
}
