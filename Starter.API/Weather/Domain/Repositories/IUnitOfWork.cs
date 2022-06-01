namespace Starter.API.Weather.Domain.Repositories;

public interface IUnitOfWork {
  Task Complete();
}
