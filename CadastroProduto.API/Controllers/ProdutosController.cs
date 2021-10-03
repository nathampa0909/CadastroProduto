using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CadastroProduto.API;
using CadastroProduto.API.Models;
using System.Text;
using System.Net.Http;

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

        // GET: api/Produtos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> GetProduto(string id)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
            {
                return NotFound();
            }

            return produto;
        }

        // PUT: api/Produtos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduto(string id, Produto produto)
        {
            if (id != produto.Id)
            {
                return BadRequest();
            }
            else if (!todosParametrosForamEnviados(produto, out string msg))
            {
                return BadRequest(msg);
            }

            _context.Entry(produto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CodigoProdutoExiste(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Produtos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
                if (IdProdutoExiste(produto.Id))
                {
                    return Conflict("Produto com id especificado já existe.");
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProduto", new { id = produto.Id }, produto);
        }

        // DELETE: api/Produtos/5
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
            if (produto.Codigo is null || produto.CodigoDepartamento is null || produto.Descricao is null || produto.Status is null)
            {
                var sb = new StringBuilder();
                if (produto.Codigo is null)
                {
                    sb.Append("Codigo, ");
                }

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
