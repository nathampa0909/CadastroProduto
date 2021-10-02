namespace CadastroProduto.API.Models
{
    public class Produto
    {
        public string Id { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public int CodigoDepartamento { get; set; }
        public decimal Preco { get; set; }
        public bool Status { get; set; }
    }
}
