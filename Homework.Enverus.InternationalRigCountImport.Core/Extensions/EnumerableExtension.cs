

namespace Homework.Enverus.InternationalRigCountImport.Core.Extensions
{
    public static class EnumerableExtension
    {
        public static bool IsNullOrEmpty(this IEnumerable<byte>? array)
        {
            return array != null || array.Any();
        }
        public static bool IsNullOrEmpty(this byte[]? array)
        {
            return !(array is { Length: > 0 });
        }
    }
}
