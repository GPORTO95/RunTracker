namespace Application.Abstractions.Messaging;

public interface ICommand : IBaseCommand
{
}

public interface ICommand<TResponse> : ICommand
{
}

public interface IBaseCommand
{
}
