using Microsoft.AspNetCore.Mvc;
using API.Services;
using API.Dtos.Produto;
using AutoMapper;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutosController : ControllerBase
{
    private readonly ProdutoService _service;
    private readonly ILogger<ProdutosController> _logger;
    private readonly IMapper _mapper;

    public ProdutosController(ProdutoService service, ILogger<ProdutosController> logger, IMapper mapper)
    {
        _service = service;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<object>>> GetAll()
    {
        try
        {
            _logger.LogInformation("GET /api/produtos - Listando todos os produtos");
            var produtos = await _service.GetAllProdutosAsync();
            return Ok(produtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar produtos");
            return StatusCode(500, new { message = "Erro ao listar produtos", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<object>> GetById(Guid id)
    {
        try
        {
            _logger.LogInformation("GET /api/produtos/{id} - Buscando produto {ProdutoId}", id);
            var produto = await _service.GetProdutoByIdAsync(id);

            if (produto == null)
                return NotFound(new { message = $"Produto com ID {id} não encontrado" });

            return Ok(produto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar produto {ProdutoId}", id);
            return StatusCode(500, new { message = "Erro ao buscar produto", error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<CreateProdutoResponse>> Create([FromBody] CreateProdutoRequest request)
    {
        try
        {
            _logger.LogInformation("POST /api/produtos - Criando novo produto: {Nome}", request.Nome);

            var produtoId = await _service.CreateProdutoAsync(
                request.Nome,
                request.Descricao,
                request.Preco,
                request.QuantidadeEstoque
            );

            return CreatedAtAction(nameof(GetById), new { id = produtoId },
                new CreateProdutoResponse { ProdutoId = produtoId, Message = "Produto criado com sucesso" });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Erro de validação ao criar produto: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar produto");
            return StatusCode(500, new { message = "Erro ao criar produto", error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProdutoRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation("PUT /api/produtos/{id} - Atualizando produto {ProdutoId}", id);

            await _service.UpdateProdutoAsync(
                id,
                request.Nome,
                request.Descricao,
                request.Preco,
                request.QuantidadeEstoque
            );

            return Ok(new { message = "Produto atualizado com sucesso", produtoId = id });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Erro de validação ao atualizar produto: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning("Produto não encontrado: {Message}", ex.Message);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar produto {ProdutoId}", id);
            return StatusCode(500, new { message = "Erro ao atualizar produto", error = ex.Message });
        }
    }
}
