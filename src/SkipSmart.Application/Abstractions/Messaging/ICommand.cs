using MediatR;
using SkipSmart.Domain.Abstractions;

namespace SkipSmart.Application.Abstractions.Messaging;

public interface ICommand : IRequest<Result>, IBaseCommand {
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand {
}

public interface IBaseCommand {
}