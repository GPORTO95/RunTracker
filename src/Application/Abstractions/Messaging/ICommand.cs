using MediatR;
using SharedKernel;

namespace Application.Abstractions.Messaging;

public interface ITransactionalCommand : ICommand;

public interface ITransactionalCommand<TResponse> : ICommand<TResponse>, ITransactionalCommand;

public interface ICommand : IRequest<Result>, IBaseCommand;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand;

public interface IBaseCommand;
