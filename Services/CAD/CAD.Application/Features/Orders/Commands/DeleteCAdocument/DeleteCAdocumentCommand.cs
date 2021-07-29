using MediatR;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder
{
    public class DeleteCAdocumentCommand : IRequest
    {
        public int Id { get; set; }
    }
}
