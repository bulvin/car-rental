using MediatR;

namespace CarRental.Common.CQRS;

public interface IQuery<out TResponse> : IRequest<TResponse> 
    where TResponse : notnull;