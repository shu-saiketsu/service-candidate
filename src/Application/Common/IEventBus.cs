using MediatR;

namespace Saiketsu.Service.Candidate.Application.Common;

public interface IEventBus
{
    /**
     * Provides an implementation to publish an integration
     * event to the event bus.
     */
    void Publish(IRequest @event);

    /**
     * Subscribe to an incoming integration event to allow
     * eventual consistency within the application.
     */
    void Subscribe<T>() where T : IRequest;
}