using MediatR;
using SkipSmart.Domain.Abstractions;

namespace SkipSmart.Application.Abstractions.Messaging;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>> 
    where TQuery : IQuery<TResponse> {
}