namespace ABMTech.Api.Services;

public class OrcamentoService
{
    public decimal CalcularTotal(decimal precoPeca, decimal valorMaoDeObra, decimal porcentagemLucro)
    {
        // Exemplo: Peça R$ 180 + (180 * 0.20) + R$ 100 mão de obra
        decimal lucroPeca = precoPeca * porcentagemLucro;
        decimal valorTotal = precoPeca + lucroPeca + valorMaoDeObra;
        
        return valorTotal;
    }

    public string GerarCodigoRastreioUnico()
    {
        // Gera um código simples tipo ABM-7392
        var random = new Random();
        return $"ABM-{random.Next(1000, 9999)}";
    }
}