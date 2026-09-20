namespace ABMTech.Api.Models;

public class PecaEstoque
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;       // ex: "Tela OLED A35"
    public string ModeloCompativel { get; set; } = string.Empty; // ex: "Galaxy A35"
    public int Quantidade { get; set; }
    public decimal PrecoCusto { get; set; }                 // Preço do fornecedor
}