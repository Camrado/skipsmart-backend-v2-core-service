using MediatR;
using SkipSmart.Domain.Abstractions;

namespace SkipSmart.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>> {
}