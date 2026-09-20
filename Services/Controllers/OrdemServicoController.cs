using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ABMTech.Api.Data;
using ABMTech.Api.Models;

namespace ABMTech.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdemServicoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdemServicoController(AppDbContext context)
        {
            _context = context;
        }

        // 1. POST: api/OrdemServico/agendar (Usado pelo cliente no agendar.html)
        [HttpPost("agendar")]
        public async Task<IActionResult> Agendar([FromBody] OrdemServico os)
        {
            if (string.IsNullOrWhiteSpace(os.NomeCliente) || 
                string.IsNullOrWhiteSpace(os.WhatsappCliente) || 
                string.IsNullOrWhiteSpace(os.ModeloAparelho))
            {
                return BadRequest(new { mensagem = "Preencha todos os campos obrigatórios." });
            }

            // Configurações iniciais do agendamento
            os.CodigoRastreio = "ABM-" + new Random().Next(1000, 9999);
            os.Status = StatusManutencao.Agendado;
            os.DataCriacao = DateTime.Now;

            // Salva no EF Core
            _context.OrdensServico.Add(os);
            await _context.SaveChangesAsync();

            return Ok(new { codigoRastreio = os.CodigoRastreio });
        }

        // 2. GET: api/OrdemServico/status/ABM-1234 (Usado pelo cliente no status.html)
        [HttpGet("status/{codigo}")]
        public async Task<IActionResult> ConsultarStatus(string codigo)
        {
            var os = await _context.OrdensServico
                .FirstOrDefaultAsync(x => x.CodigoRastreio.ToLower() == codigo.ToLower().Trim());

            if (os == null)
            {
                return NotFound(new { mensagem = "Ordem de Serviço não encontrada com este código." });
            }

            return Ok(new { 
                codigoRastreio = os.CodigoRastreio,
                status = FormatStatus(os.Status),
                statusEnum = os.Status.ToString(),
                nomeCliente = os.NomeCliente,
                modeloAparelho = os.ModeloAparelho,
                defeitoRelatado = os.DefeitoRelatado,
                valorTotal = os.ValorTotalCalculado,
                dataCriacao = os.DataCriacao.ToString("dd/MM/yyyy HH:mm")
            });
        }

        // 3. GET: api/OrdemServico/todas (Usado pelo técnico/painel para "ver o banco de dados")
        [HttpGet("todas")]
        public async Task<IActionResult> ObterTodas()
        {
            var ordens = await _context.OrdensServico
                .OrderByDescending(x => x.DataCriacao)
                .Select(os => new {
                    id = os.Id,
                    codigoRastreio = os.CodigoRastreio,
                    nomeCliente = os.NomeCliente,
                    whatsappCliente = os.WhatsappCliente,
                    modeloAparelho = os.ModeloAparelho,
                    defeitoRelatado = os.DefeitoRelatado,
                    status = os.Status.ToString(),
                    precoPeca = os.PrecoPecaFornecedor,
                    maoDeObra = os.ValorMaoDeObra,
                    valorTotal = os.ValorTotalCalculado,
                    dataCriacao = os.DataCriacao.ToString("dd/MM/yyyy HH:mm")
                })
                .ToListAsync();

            return Ok(ordens);
        }

        // 4. PUT: api/OrdemServico/atualizar/1 (Usado no Painel do Técnico para alterar status e preço)
        [HttpPut("atualizar/{id}")]
        public async Task<IActionResult> AtualizarStatusEOrcamento(int id, [FromBody] AtualizarOSDTO dados)
        {
            var os = await _context.OrdensServico.FindAsync(id);

            if (os == null)
            {
                return NotFound(new { mensagem = "Ordem de serviço não encontrada." });
            }

            // Atualiza o status
            os.Status = dados.NovoStatus;
            
            // Atualiza orçamentos se fornecidos
            os.PrecoPecaFornecedor = dados.PrecoPecaFornecedor;
            os.ValorMaoDeObra = dados.ValorMaoDeObra;
            
            // Calcula valor total com margem da peça (20%)
            decimal valorPecaComMargem = os.PrecoPecaFornecedor * (1 + os.MargemLucroPecaPorcentagem);
            os.ValorTotalCalculado = valorPecaComMargem + os.ValorMaoDeObra;

            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Ordem de Serviço atualizada com sucesso!", valorTotal = os.ValorTotalCalculado });
        }

        // Método auxiliar para formatar o texto do status para o cliente
        private static string FormatStatus(StatusManutencao status)
        {
            return status switch
            {
                StatusManutencao.Agendado => "Agendado (Aguardando entrega do aparelho)",
                StatusManutencao.AguardandoAprovacao => "Aguardando Aprovação do Orçamento",
                StatusManutencao.Aprovado => "Orçamento Aprovado",
                StatusManutencao.EmAndamento => "Em Manutenção pelo Técnico",
                StatusManutencao.TestesFinalizados => "Em Testes Finais",
                StatusManutencao.ProntoParaRetirada => "Pronto para Retirada!",
                StatusManutencao.Entregue => "Aparelho Entregue",
                StatusManutencao.Cancelado => "Serviço Cancelado",
                _ => "Em Processamento"
            };
        }
    }

    // DTO para receber os dados de atualização enviados pelo painel do técnico
    public class AtualizarOSDTO
    {
        public StatusManutencao NovoStatus { get; set; }
        public decimal PrecoPecaFornecedor { get; set; }
        public decimal ValorMaoDeObra { get; set; }
    }
}