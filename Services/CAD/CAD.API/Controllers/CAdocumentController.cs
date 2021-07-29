using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Commands.DeleteOrder;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Ordering.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CAdocumentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CAdocumentController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        //[HttpGet("{userName}", Name = "GetCAdocument")]
        [HttpGet]
        [Route("GetCAdocument")]
        [ProducesResponseType(typeof(IEnumerable<CAdocumentVm>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<CAdocumentVm>>> GetCAdocumentByUserName(string businesspartner)
        {
            var query = new GetCAdocumentListQuery(businesspartner);
            var orders = await _mediator.Send(query);
            return Ok(orders);
        }
        // testing purpose
        // [HttpPost(Name = "InsertCAdocument")]
        [HttpPost]
        [Route("InsertCAdocument")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<int>> InsertCAdocument([FromBody] InsertCAdocumentCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        //[HttpPut(Name = "UpdateCAdocument")]
        [HttpPut]
        [Route("UpdateCAdocument")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateCAdocument([FromBody] UpdateCAdocumentCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        //[HttpDelete("{id}", Name = "DeleteCAdocument")]
        [HttpDelete]
        [Route("DeleteCAdocument")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteCAdocument(int id)
        {
            var command = new DeleteCAdocumentCommand() { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
