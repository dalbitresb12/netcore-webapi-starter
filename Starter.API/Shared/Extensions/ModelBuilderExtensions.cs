using Microsoft.EntityFrameworkCore;

namespace Starter.API.Shared.Extensions;

public static class ModelBuilderExtensions {
  public static void UseSnakeCase(this ModelBuilder builder) {
    foreach (var entity in builder.Model.GetEntityTypes()) {
      entity.SetTableName(entity.GetTableName()?.ToSnakeCase());

      foreach (var prop in entity.GetProperties()) {
        prop.SetColumnName(prop.GetColumnBaseName().ToSnakeCase());
      }

      foreach (var key in entity.GetKeys()) {
        key.SetName(key.GetName()?.ToSnakeCase());
      }

      foreach (var fk in entity.GetForeignKeys()) {
        fk.SetConstraintName(fk.GetConstraintName()?.ToSnakeCase());
      }

      foreach (var index in entity.GetIndexes()) {
        index.SetDatabaseName(index.GetDatabaseName().ToSnakeCase());
      }
    }
  }
}
