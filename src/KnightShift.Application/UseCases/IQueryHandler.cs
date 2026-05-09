namespace KnightShift.Application.UseCases;

public interface IQueryHandler<in TQuery, out TResult>
{
    TResult Handle(TQuery query);
}
