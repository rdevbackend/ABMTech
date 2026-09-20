namespace ABMTech.Api.Models;

public class ServicoMaoDeObra
{
    public int Id { get; set; }
    public string NomeServico { get; set; } = string.Empty; // ex: "Troca de Tela", "Limpeza"
    public decimal ValorMaoDeObra { get; set; }             // ex: 100.00
}