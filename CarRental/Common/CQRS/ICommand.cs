using MediatR;

namespace CarRental.Common.CQRS;

public interface ICommand : IRequest<Unit>;

public interface ICommand<out TResponse> : IRequest<TResponse>;