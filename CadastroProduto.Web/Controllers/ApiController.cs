using CadastroProduto.Web.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;

namespace CadastroProduto.Web.Controllers
{
    public class ApiController
    {
        static HttpClient httpClient = new();
        const string URL_API = "http://localhost:34070";

        public async Task<List<ProdutoModel>> ObtenhaTodosProdutos()
        {
            var response = await httpClient.GetAsync(URL_API + "/api/produtos");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<List<ProdutoModel>>();
            }

            return null;
        }

        public async Task<ProdutoModel> ObtenhaProduto(string id)
        {
            var response = await httpClient.GetAsync(URL_API + "/api/produtos/" + id);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<ProdutoModel>();
            }

            return null;
        }

        public async Task<List<DepartamentoModel>> ObtenhaDepartamentos()
        {
            var response = await httpClient.GetAsync("https://private-anon-9924aff2ed-maximatech.apiary-mock.com/fullstack/departamento");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<List<DepartamentoModel>>();
            }

            return null;
        }

        public async Task<HttpResponseMessage> InsiraProduto(ProdutoModel produto)
        {
            var produtoConvertido = JsonConvert.SerializeObject(produto);
            var httpContent = new StringContent(produtoConvertido, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(URL_API + "/api/produtos", httpContent);

            if (response.IsSuccessStatusCode)
            {
                return response;
            }

            return null;
        }

        public async Task<HttpResponseMessage> AtualizeProduto(ProdutoModel produto)
        {
            var produtoConvertido = JsonConvert.SerializeObject(produto);
            var httpContent = new StringContent(produtoConvertido, Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync(URL_API + "/api/produtos/" + produto.Id, httpContent);

            if (response.IsSuccessStatusCode)
            {
                return response;
            }

            return null;
        }

        public async Task<HttpResponseMessage> DeleteProduto(string id)
        {
            var response = await httpClient.DeleteAsync(URL_API + "/api/produtos/" + id);

            if (response.IsSuccessStatusCode)
            {
                return response;
            }

            return null;
        }
    }
}
