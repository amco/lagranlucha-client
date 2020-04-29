using System.Collections.Generic;
using System.Linq;

namespace CFLFramework.Utilities.Extensions
{
    public static partial class ArrayExtensions
    {
        #region BEHAVIORS

        public static bool IsEqual<T>(this IEnumerable<T> collection, IEnumerable<T> otherCollection)
        {
            return Enumerable.SequenceEqual(collection, otherCollection);
        }

        #endregion
    }
}
