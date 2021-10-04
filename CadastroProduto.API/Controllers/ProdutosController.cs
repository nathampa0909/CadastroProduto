using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CadastroProduto.API.Models;
using System.Text;

namespace CadastroProduto.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly ProdutoDbContext _context;

        public ProdutosController(ProdutoDbContext context)
        {
            _context = context;
        }

        // GET: api/Produtos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos()
        {
            return await _context.Produtos.ToListAsync();
        }

        // GET: api/Produtos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> GetProduto(string id)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
            {
                return NotFound("Produto não encontrado.");
            }

            return produto;
        }

        // PUT: api/Produtos/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduto(string id, Produto produto)
        {
            if (!todosParametrosForamEnviados(produto, out string msg))
            {
                return BadRequest(msg);
            }

            if (!IdProdutoExiste(id))
            {
                return NotFound("Produto não encontrado.");
            }

            var produtoAntigo = _context.Produtos.AsNoTracking().First(prdt => prdt.Id == id);

            if (id != produtoAntigo.Id || produto.Id != produtoAntigo.Id || produto.Codigo != produtoAntigo.Codigo)
            {
                return BadRequest("Não é permitido alterar ID ou código do produto.");
            }

            _context.Entry(produto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        // POST: api/Produtos
        [HttpPost]
        public async Task<ActionResult<Produto>> PostProduto(Produto produto)
        {
            if (!todosParametrosForamEnviados(produto, out string msg))
            {
                return BadRequest(msg);
            }

            var uuid = Guid.NewGuid().ToString();
            var uuidEhRepetido = (await _context.Produtos.FindAsync(uuid)) != null;

            while (uuidEhRepetido)
            {
                uuid = Guid.NewGuid().ToString();
                uuidEhRepetido = (await _context.Produtos.FindAsync(uuid)) != null;
            }

            var ultimoCodigoProduto = 0;

            if (_context.Produtos.Any())
            {
                ultimoCodigoProduto = int.Parse(_context.Produtos.OrderBy(p => p.Codigo).Last().Codigo);
            }

            produto.Id = uuid;
            produto.Codigo = (ultimoCodigoProduto + 1).ToString();

            _context.Produtos.Add(produto);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {

                throw;
            }

            return CreatedAtAction("GetProduto", new { id = produto.Id }, produto);
        }

        // DELETE: api/Produtos/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(string id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IdProdutoExiste(string id)
        {
            return _context.Produtos.Any(e => e.Id == id);
        }

        private bool CodigoProdutoExiste(string codigo)
        {
            return _context.Produtos.Any(e => e.Codigo == codigo);
        }

        private bool todosParametrosForamEnviados(Produto produto, out string msg)
        {
            if (produto.CodigoDepartamento is null || produto.Descricao is null || produto.Status is null)
            {
                var sb = new StringBuilder();

                if (produto.CodigoDepartamento is null)
                {
                    sb.Append("CodigoDepartamento, ");
                }

                if (produto.Descricao is null)
                {
                    sb.Append("Descricao, ");
                }

                if (produto.Status is null)
                {
                    sb.Append("Status, ");
                }

                msg = $"Os seguintes parâmetros estão faltando: {sb.ToString().Remove(sb.ToString().Length - 2)}.";

                return false;
            }

            msg = null;
            return true;
        }
    }
}
