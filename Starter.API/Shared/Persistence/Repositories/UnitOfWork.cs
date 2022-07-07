using Starter.API.Shared.Persistence.Contexts;
using Starter.API.Weather.Domain.Repositories;

namespace Starter.API.Shared.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork {
  private readonly AppDbContext context;

  public UnitOfWork(AppDbContext context) {
    this.context = context;
  }

  public async Task Complete() {
    await context.SaveChangesAsync();
  }
}
