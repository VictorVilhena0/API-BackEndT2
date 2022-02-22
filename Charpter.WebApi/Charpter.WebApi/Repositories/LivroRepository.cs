using Charpter.WebApi.Contexts;
using Charpter.WebApi.Models;

namespace Charpter.WebApi.Repositories
{
    public class LivroRepository
    {
        private readonly CharpterContext _context;

        public LivroRepository(CharpterContext context)
        {
            _context = context;
        }

        public List<Livro> Listar()
        {
            return _context.Livros.ToList();
        }
    }
}
