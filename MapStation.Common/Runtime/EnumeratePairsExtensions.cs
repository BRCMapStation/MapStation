using System.Collections.Generic;

namespace MapStation.Common {
    public static class EnumeratePairsExtensions
    {
        public static IEnumerable<KeyValuePair<int, T>> Pairs<T>(this T[] array) {
            for(int i = 0; i < array.Length; i++) {
                yield return new KeyValuePair<int, T>(i, array[i]);
            }
        }
        public static IEnumerable<KeyValuePair<int, T>> Pairs<T>(this List<T> list) {
            for(int i = 0; i < list.Count; i++) {
                yield return new KeyValuePair<int, T>(i, list[i]);
            }
        }
    }
}