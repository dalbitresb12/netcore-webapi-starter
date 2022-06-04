using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Starter.API.Shared.Extensions;

public static class ModelStateExtensions {
  public static List<string> GetErrorMessages(this ModelStateDictionary dictionary) {
    var filtered = dictionary.Where(m => m.Value != null) as IEnumerable<KeyValuePair<string, ModelStateEntry>>;
    return filtered.SelectMany(m => m.Value.Errors).Select(m => m.ErrorMessage).ToList();
  }
}
