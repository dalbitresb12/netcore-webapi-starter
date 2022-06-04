using System.Reflection;

namespace Starter.API.Shared.Extensions;

public static class ObjectExtensions {
  // Adapted from https://stackoverflow.com/a/8724150/15040387
  public static void CopyProperties(this object? source, object? destination, string[]? ignoredKeys = null) {
    if (source is null || destination is null) {
      throw new Exception("Source or/and destination objects are null");
    }

    var typeDest = destination.GetType();
    var typeSrc = source.GetType();
    var srcProps = typeSrc.GetProperties();
    foreach (var srcProp in srcProps) {
      if (ignoredKeys != null && ignoredKeys.Contains(srcProp.Name)) continue;
      if (!srcProp.CanRead) continue;
      var targetProp = typeDest.GetProperty(srcProp.Name);
      if (targetProp is null) continue;
      if (!targetProp.CanWrite) continue;
      var method = targetProp.GetSetMethod(true);
      if (method != null && method.IsPrivate) continue;
      if (method != null && (method.Attributes & MethodAttributes.Abstract) != 0) continue;
      if (!targetProp.PropertyType.IsAssignableFrom(srcProp.PropertyType)) continue;
      // Passed all tests, lets set the value
      targetProp.SetValue(destination, srcProp.GetValue(source, null), null);
    }
  }
}
