using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder
{
    public class DeleteCAdocumentCommandHandler : IRequestHandler<DeleteCAdocumentCommand>
    {
        private readonly ICAdocumentRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteCAdocumentCommandHandler> _logger;

        public DeleteCAdocumentCommandHandler(ICAdocumentRepository orderRepository, IMapper mapper, ILogger<DeleteCAdocumentCommandHandler> logger)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(DeleteCAdocumentCommand request, CancellationToken cancellationToken)
        {
            var orderToDelete = await _orderRepository.GetByIdAsync(request.Id);
            if (orderToDelete == null)
            {
                throw new NotFoundException(nameof(CAdocument), request.Id);
            }            

            await _orderRepository.DeleteAsync(orderToDelete);

            _logger.LogInformation($"Order {orderToDelete.id} is successfully deleted.");
           // _logger.LogInformation($"Order {orderToDelete.Id} is successfully deleted.");

            return Unit.Value;
        }
    }
}
