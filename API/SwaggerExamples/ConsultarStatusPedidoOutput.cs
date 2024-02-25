using Domain.Pedidos;
using Application.Pedidos.Boundaries;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace API.SwaggerExamples
{
    [ExcludeFromCodeCoverage]
    public class ConsultarStatusPedidoOutputExample : IExamplesProvider<ConsultarStatusPedidoOutput>
    {
        public ConsultarStatusPedidoOutput GetExamples()
        {
            return new ConsultarStatusPedidoOutput
            {
                PedidoId = Guid.NewGuid(),
                Status = PedidoStatus.Iniciado
            };
        }
    }
}
