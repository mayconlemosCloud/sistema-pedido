namespace API.Events;

/// <summary>
/// Interface para publicação de eventos
/// Permite mocking nos testes
/// </summary>
public interface IEventPublisher
{
    /// <summary>
    /// Publica um evento de Pedido Criado
    /// </summary>
    void PublishEvent(PedidoCriadoEvent @event);
}
