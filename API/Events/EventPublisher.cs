using Microsoft.Extensions.Logging;

namespace API.Events;

/// <summary>
/// Publicador de eventos (mock simples)
/// Simula a publicação de eventos como se fosse Kafka/RabbitMQ
/// </summary>
public class EventPublisher
{
    private readonly ILogger<EventPublisher> _logger;

    public EventPublisher(ILogger<EventPublisher> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Publica um evento de Pedido Criado
    /// </summary>
    public void PublishEvent(PedidoCriadoEvent @event)
    {
        _logger.LogInformation(
            "📢 [EVENTO PUBLICADO] PedidoCriado | " +
            "PedidoId: {PedidoId} | " +
            "ClienteId: {ClienteId} | " +
            "ValorTotal: {ValorTotal} | " +
            "DataCriacao: {DataCriacao}",
            @event.PedidoId,
            @event.ClienteId,
            @event.ValorTotal,
            @event.DataCriacao
        );

        // Aqui seria feita a integração real com Kafka/RabbitMQ
        // Por enquanto, apenas logamos a publicação
    }
}
