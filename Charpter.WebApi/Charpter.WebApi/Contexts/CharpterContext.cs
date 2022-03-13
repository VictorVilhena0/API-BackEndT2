using Microsoft.EntityFrameworkCore;
using Charpter.WebApi.Models;

namespace Charpter.WebApi.Contexts
{
    public class CharpterContext : DbContext
    {
        public CharpterContext()
        {
        }

        public CharpterContext(DbContextOptions<CharpterContext> options) : base(options)
        {
            
        }

        // vamos utilizar esse método para configurar o banco de dados
        protected override void
            OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // cada provedor tem sua sintaxe para especificação
                optionsBuilder.UseSqlServer("Data Source = DESKTOP-JTA7A8U; initial catalog = Chapter; Integrated Security = true");
            }
        }
        // dbset representa as entidades que serão utilizadas nas operações de leitura, criação, atualização e deleção
        public DbSet<Livro>? Livros { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
