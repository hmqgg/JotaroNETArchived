namespace Jotaro.Utils;

/// <summary>
///     This interface is designed to be a marker.
/// </summary>
public interface IEnumRecord<TEnum> where TEnum : IEnumRecord<TEnum>
{
}
