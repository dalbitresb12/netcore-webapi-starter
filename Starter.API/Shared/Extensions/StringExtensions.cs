namespace Starter.API.Shared.Extensions;

public static class StringExtensions {
  private static IEnumerable<char> Convert(CharEnumerator e) {
    if (!e.MoveNext()) yield break;

    yield return char.ToLower(e.Current);

    while (e.MoveNext()) {
      if (char.IsUpper(e.Current)) {
        yield return '_';
        yield return char.ToLower(e.Current);
      } else {
        yield return e.Current;
      }
    }
  }

  public static string ToSnakeCase(this string text) {
    return new string(Convert(text.GetEnumerator()).ToArray());
  }
}
