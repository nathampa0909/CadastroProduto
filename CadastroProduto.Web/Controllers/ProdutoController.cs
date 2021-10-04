using CadastroProduto.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace CadastroProduto.Web.Controllers
{
    public class ProdutoController : Controller
    {
        readonly ApiController api = new();

        public async Task<ActionResult> Index()
        {
            var produtos = await api.ObtenhaTodosProdutos(); 
            var departamentos = await api.ObtenhaDepartamentos();

            ViewBag.ListaDepartamentos = departamentos;

            return View(produtos.OrderBy(produto => produto.Codigo));
        }

        public async Task<ActionResult> Criar()
        {
            var departamentos = await api.ObtenhaDepartamentos();
            var selectListDepartamentos = new SelectList(departamentos, nameof(DepartamentoModel.Codigo), nameof(DepartamentoModel.Nome));

            ViewBag.ListaDepartamentos = selectListDepartamentos;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Criar(IFormCollection collection)
        {
            var produto = new ProdutoModel
            {
                Descricao = collection.First(c => c.Key.Equals("Descricao")).Value.ToString(),
                CodigoDepartamento = collection.First(c => c.Key.Equals("CodigoDepartamento")).Value.ToString(),
                Preco = decimal.Parse(collection.First(c => c.Key.Equals("Preco")).Value.ToString()),
                Status = collection.First(c => c.Key.Equals("Status")).Value.ToString().Equals("0")
            };

            await api.InsiraProduto(produto);

            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> Editar(string id)
        {
            var departamentos = await api.ObtenhaDepartamentos();
            var selectListDepartamentos = new SelectList(departamentos, nameof(DepartamentoModel.Codigo), nameof(DepartamentoModel.Nome));

            ViewBag.ListaDepartamentos = selectListDepartamentos;

            var produto = await api.ObtenhaProduto(id);
            return View(produto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar(string id, IFormCollection collection)
        {
            var produto = new ProdutoModel
            {
                Id = id,
                Codigo = collection.First(c => c.Key.Equals("Codigo")).Value.ToString(),
                Descricao = collection.First(c => c.Key.Equals("Descricao")).Value.ToString(),
                CodigoDepartamento = collection.First(c => c.Key.Equals("CodigoDepartamento")).Value.ToString(),
                Preco = decimal.Parse(collection.First(c => c.Key.Equals("Preco")).Value.ToString()),
                Status = collection.First(c => c.Key.Equals("Status")).Value.ToString().Equals("0")
            };

            await api.AtualizeProduto(produto);

            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> Deletar(string id)
        {
            var produto = await api.ObtenhaProduto(id);
            return View(produto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Deletar(string id, IFormCollection collection)
        {
            await api.DeleteProduto(id);
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
