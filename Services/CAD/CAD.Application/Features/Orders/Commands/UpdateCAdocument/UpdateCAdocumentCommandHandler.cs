using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateCAdocumentCommandHandler : IRequestHandler<UpdateCAdocumentCommand>
    {
        private readonly ICAdocumentRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCAdocumentCommandHandler> _logger;

        public UpdateCAdocumentCommandHandler(ICAdocumentRepository orderRepository, IMapper mapper, ILogger<UpdateCAdocumentCommandHandler> logger)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(UpdateCAdocumentCommand request, CancellationToken cancellationToken)
        {
            var orderToUpdate = await _orderRepository.GetByIdAsync(request.Id);
            if (orderToUpdate == null)
            {
                throw new NotFoundException(nameof(CAdocument), request.Id);
            }
            
            _mapper.Map(request, orderToUpdate, typeof(UpdateCAdocumentCommand), typeof(CAdocument));

            await _orderRepository.UpdateAsync(orderToUpdate);

           // _logger.LogInformation($"Order {orderToUpdate.Id} is successfully updated.");
            _logger.LogInformation($"Order {orderToUpdate.id} is successfully updated.");

            return Unit.Value;
        }
    }
}
