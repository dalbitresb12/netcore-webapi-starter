using Starter.API.Shared.Persistence.Contexts;

namespace Starter.API.Shared.Persistence.Repositories;

public class BaseRepository {
  protected readonly AppDbContext context;

  public BaseRepository(AppDbContext context) {
    this.context = context;
  }
}
