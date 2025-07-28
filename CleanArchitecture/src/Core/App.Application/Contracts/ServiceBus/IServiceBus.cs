using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Domain.Events;

namespace App.Application.Contracts.ServiceBus
{
    public interface IServiceBus
    {
        Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : IEventOrMessage;
        Task SendAsync<T>(T message, string queueName, CancellationToken cancellationToken = default) where T : IEventOrMessage;
    }
}
