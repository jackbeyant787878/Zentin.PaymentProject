using MediatR;
using Microsoft.AspNetCore.Mvc;
using Zentin.Payment.Api.Auth;
using Zentin.Payment.Application.Commands.PayOrders;
using Zentin.Payment.Application.Queries.PayOrders;

namespace Zentin.Payment.Api.Controllers
{
    [ApiController]
    [Route("api/payorders")]
    public sealed class PayOrderController(IMediator mediator) : ControllerBase
    {
        public sealed record MarkPayingBody(string IfCode);
        public sealed record ConfirmSuccessBody(string ChannelOrderNo, string? ChannelUser = null, string? PlatformOrderNo = null);
        public sealed record ConfirmFailBody(string ErrCode, string ErrMsg);
        public sealed record CloseBody(string? Reason = null);

        [HttpPost]
        [Permission("pay:order:create")]
        public async Task<IActionResult> Create([FromBody] CreatePayOrderCommand command, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = result.PayOrderId }, result);
        }

        [HttpGet("{id}")]
        [Permission("pay:order:query")]
        public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
        {
            var dto = await mediator.Send(new GetPayOrderQuery(id), cancellationToken);
            return Ok(dto);
        }

        [HttpPost("{id}/paying")]
        [Permission("pay:order:paying")]
        public async Task<IActionResult> MarkPaying(string id, [FromBody] MarkPayingBody body, CancellationToken cancellationToken)
        {
            await mediator.Send(new MarkPayOrderPayingCommand(id, body.IfCode), cancellationToken);
            return NoContent();
        }

        [HttpPost("{id}/confirm-success")]
        [Permission("pay:order:confirm")]
        public async Task<IActionResult> ConfirmSuccess(string id, [FromBody] ConfirmSuccessBody body, CancellationToken cancellationToken)
        {
            await mediator.Send(new ConfirmPayOrderSuccessCommand(id, body.ChannelOrderNo, body.ChannelUser, body.PlatformOrderNo), cancellationToken);
            return NoContent();
        }

        [HttpPost("{id}/confirm-fail")]
        [Permission("pay:order:confirm")]
        public async Task<IActionResult> ConfirmFail(string id, [FromBody] ConfirmFailBody body, CancellationToken cancellationToken)
        {
            await mediator.Send(new ConfirmPayOrderFailCommand(id, body.ErrCode, body.ErrMsg), cancellationToken);
            return NoContent();
        }

        [HttpPost("{id}/close")]
        [Permission("pay:order:close")]
        public async Task<IActionResult> Close(string id, [FromBody] CloseBody? body, CancellationToken cancellationToken)
        {
            await mediator.Send(new ClosePayOrderCommand(id, body?.Reason), cancellationToken);
            return NoContent();
        }
    }
}