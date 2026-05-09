namespace KnightShift.Application.UseCases;

public interface ICommandHandler<in TCommand>
{
    void Handle(TCommand command);
}
