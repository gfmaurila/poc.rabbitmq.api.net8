using Microsoft.EntityFrameworkCore;
using poc.api.sqlserver.Model;

namespace poc.api.sqlserver.Configuration;

public class SqlServerDb : DbContext
{
    public SqlServerDb(DbContextOptions<SqlServerDb> op) : base(op)
    { }

    public DbSet<Produto> Produto { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Produto>().HasData(
            new Produto
            {
                Id = 1,
                Nome = "Dell not",
                Preco = 10000
            },
            new Produto
            {
                Id = 2,
                Nome = "Dell Teclado",
                Preco = 199
            },
            new Produto
            {
                Id = 3,
                Nome = "Dell mouse",
                Preco = 120
            },
            new Produto
            {
                Id = 4,
                Nome = "Dell monitor",
                Preco = 1985
            }
        );
        base.OnModelCreating(builder);
    }
}
