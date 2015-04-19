using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Impostor.Support {
    internal static class DictionaryExtensions {
        [CanBeNull]
        public static TValue GetValueOrDefault<TKey, TValue>([NotNull] this IReadOnlyDictionary<TKey, TValue> dictionary, [NotNull] TKey key) {
            TValue value;
            dictionary.TryGetValue(key, out value);
            return value;
        }
    }
}
