using Starter.API.Weather.Persistence.Contexts;

namespace Starter.API.Weather.Persistence.Repositories;

public class BaseRepository {
  protected readonly AppDbContext context;

  public BaseRepository(AppDbContext context) {
    this.context = context;
  }
}
