using System.Reflection;

namespace Jotaro.Utils;

public abstract record EnumRecord<TEnum, TKey>(TKey Key) : IEnumRecord<TEnum> where TEnum : EnumRecord<TEnum, TKey>
    where TKey : IEquatable<TKey>, IComparable<TKey>
{
    private static readonly Lazy<TEnum[]> sEnumerations =
        new(GetAllEnumerations, LazyThreadSafetyMode.ExecutionAndPublication);

    private static readonly Lazy<Dictionary<TKey, TEnum>> sFromKey = new(sEnumerations.Value.ToDictionary(e => e.Key));

    private static readonly Lazy<IReadOnlyCollection<TEnum>> sList = new(sFromKey.Value.Values.ToList().AsReadOnly());

    public static IReadOnlyCollection<TEnum> List => sList.Value;

    public static TEnum FromKey(TKey key) => sFromKey.Value.TryGetValue(key, out var result)
        ? result
        : throw new KeyNotFoundException($"Key {key} is not found.");

    public static bool TryFromKey(TKey key, out TEnum? result) => sFromKey.Value.TryGetValue(key, out result);

    public static TEnum? FirstOrDefault(Func<TEnum, bool> predicate) => sFromKey.Value.Values.FirstOrDefault(predicate);

    private static TEnum[] GetAllEnumerations()
    {
        var baseType = typeof(TEnum);
        var ifType = typeof(IEnumRecord<TEnum>);

        // Get all types implements the marker Interface.
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => ifType.IsAssignableFrom(t) && !t.IsInterface)
            .SelectMany(t => t.GetFields(BindingFlags.Public | BindingFlags.Static))
            .Where(f => baseType.IsAssignableFrom(f.FieldType))
            .Select(f => f.GetValue(null)!)
            .Cast<TEnum>()
            .ToArray();
    }
}
