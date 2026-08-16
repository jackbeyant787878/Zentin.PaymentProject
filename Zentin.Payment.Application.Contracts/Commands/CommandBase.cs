using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zentin.Payment.Application.Contracts.Commands
{
   
    public abstract class CommandBase : IRequest
    {
        /// <summary>
        /// request trace id, used for logging and tracing
        /// </summary>
        public string RequestId { get; init; } = Guid.NewGuid().ToString("N");
    }

  
    public abstract class CommandBase<TResponse> : IRequest<TResponse>
    {
        public string RequestId { get; init; } = Guid.NewGuid().ToString("N");
    }
}
