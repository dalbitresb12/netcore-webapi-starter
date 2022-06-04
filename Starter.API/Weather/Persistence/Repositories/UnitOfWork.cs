using Starter.API.Weather.Domain.Repositories;
using Starter.API.Weather.Persistence.Contexts;

namespace Starter.API.Weather.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork {
  private readonly AppDbContext context;

  public UnitOfWork(AppDbContext context) {
    this.context = context;
  }

  public async Task Complete() {
    await context.SaveChangesAsync();
  }
}
