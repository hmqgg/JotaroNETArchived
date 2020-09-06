using System;

namespace Jotaro.Repository.Paginate
{
    internal static class ThrowHelper
    {
        internal static void ThrowArgumentOutOfRangeExceptionWhenCheckFailed(int size, int index)
        {
            if (size < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(size), "Page size must be greater than 0");
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Page index must be greater than 0");
            }
        }
    }
}
