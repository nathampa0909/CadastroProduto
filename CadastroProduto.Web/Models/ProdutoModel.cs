using System.ComponentModel.DataAnnotations;

namespace CadastroProduto.Web.Models
{
    public class ProdutoModel
    {
        public string Id { get; set; }

        [Display(Name = "Código")]
        public string Codigo { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Departamento")]
        public string CodigoDepartamento { get; set; }

        [Display(Name = "Preço")]
        public decimal Preco { get; set; }

        public bool? Status { get; set; }
    }
}
