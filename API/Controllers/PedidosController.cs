using Microsoft.AspNetCore.Mvc;
using API.Services;
using API.Dtos.Pedido;
using AutoMapper;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidosController : ControllerBase
{
    private readonly PedidoService _service;
    private readonly ILogger<PedidosController> _logger;
    private readonly IMapper _mapper;

    public PedidosController(PedidoService service, ILogger<PedidosController> logger, IMapper mapper)
    {
        _service = service;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<PedidoResponse>>> GetAll()
    {
        try
        {
            _logger.LogInformation("GET /api/pedidos - Listando todos os pedidos");
            var pedidos = await _service.GetAllPedidosAsync();
            var response = _mapper.Map<List<PedidoResponse>>(pedidos);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar pedidos");
            return StatusCode(500, new { message = "Erro ao listar pedidos", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PedidoResponse>> GetById(Guid id)
    {
        try
        {
            _logger.LogInformation("GET /api/pedidos/{id} - Buscando pedido {PedidoId}", id);
            var pedido = await _service.GetPedidoByIdAsync(id);

            if (pedido == null)
                return NotFound(new { message = $"Pedido com ID {id} não encontrado" });

            return Ok(_mapper.Map<PedidoResponse>(pedido));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar pedido {PedidoId}", id);
            return StatusCode(500, new { message = "Erro ao buscar pedido", error = ex.Message });
        }
    }

    [HttpGet("cliente/{clienteId}")]
    public async Task<ActionResult<List<PedidoResponse>>> GetByClienteId(Guid clienteId)
    {
        try
        {
            _logger.LogInformation("GET /api/pedidos/cliente/{clienteId} - Buscando pedidos do cliente {ClienteId}", clienteId);
            var pedidos = await _service.GetPedidosByClienteIdAsync(clienteId);
            var response = _mapper.Map<List<PedidoResponse>>(pedidos);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar pedidos do cliente {ClienteId}", clienteId);
            return StatusCode(500, new { message = "Erro ao buscar pedidos", error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<CreatePedidoResponse>> Create([FromBody] CreatePedidoRequest request)
    {
        try
        {
            _logger.LogInformation("POST /api/pedidos - Criando novo pedido para cliente {ClienteId}", request.ClienteId);

            var itens = request.Itens.Select(i => (i.ProdutoId, i.Quantidade)).ToList();

            var pedidoId = await _service.CreatePedidoAsync(
                request.ClienteId,
                itens
            );

            return CreatedAtAction(nameof(GetById), new { id = pedidoId },
                new CreatePedidoResponse
                {
                    PedidoId = pedidoId,
                    Message = "Pedido criado com sucesso e evento publicado"
                });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Erro de validação ao criar pedido: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning("Erro ao criar pedido: {Message}", ex.Message);
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Erro ao criar pedido: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar pedido");
            return StatusCode(500, new { message = "Erro ao criar pedido", error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PedidoResponse>> Update(Guid id, [FromBody] UpdatePedidoRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation("PUT /api/pedidos/{id} - Atualizando pedido {PedidoId}", id);

            var itens = request.Itens.Select(i => (i.ProdutoId, i.Quantidade)).ToList();
            var pedido = await _service.UpdatePedidoAsync(id, request.Status, itens);

            if (pedido == null)
                return NotFound(new { message = $"Pedido com ID {id} não encontrado" });

            return Ok(_mapper.Map<PedidoResponse>(pedido));
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Erro de validação ao atualizar pedido: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning("Erro ao atualizar pedido: {Message}", ex.Message);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar pedido {PedidoId}", id);
            return StatusCode(500, new { message = "Erro ao atualizar pedido", error = ex.Message });
        }
    }
}
