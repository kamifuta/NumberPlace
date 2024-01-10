using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Kamifuta.MyUtil
{
    public static class IEnumerableExtension
    {
        public static T RandomGet<T>(this IEnumerable<T> list)
        {
            if (!list.Any())
            {
                throw new InvalidOperationException("—v‘f‚ª‘¶İ‚µ‚Ü‚¹‚ñ");
            }

            Random rand = new Random();
            int index = rand.Next(0, list.Count());
            return list.ElementAt(index);
        }

        public static IEnumerable<IEnumerable<T>> Combination<T>(this IEnumerable<T> list, int k)
        {
            if (k == 0)
            {
                yield return Enumerable.Empty<T>();
            }
            else
            {
                int i = 1;
                foreach (var element in list)
                {
                    var skippedList = list.Skip(i);
                    foreach (var combinationList in Combination(skippedList, k - 1))
                        yield return combinationList.Prepend(element);

                    i++;
                }
            }
        }

        public static IEnumerable<IEnumerable<T>> Permutation<T>(this IEnumerable<T> list, int k)
        {
            if (list.Count() < k)
            {
                throw new InvalidOperationException("ƒŠƒXƒg‚Ì’·‚³‚ª’Z‚·‚¬‚Ü‚·");
            }

            if (k == 1)
            {
                foreach(T element in list)
                {
                    yield return new T[] { element };
                }
            }
            else
            {
                foreach(T element in list)
                {
                    var left = new T[] { element };
                    foreach (var rightside in Permutation(list.Except(left), k - 1))
                    {
                        yield return left.Concat(rightside).ToArray();
                    }
                }
            }
        }

        /// <summary>
        /// Compare‚Ì‚·‚×‚Ä‚Ì—v‘f‚ğlist‚ª‚Á‚Ä‚¢‚é‚©
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="compare"></param>
        /// <returns></returns>
        public static bool Any<T>(this IEnumerable<T> list, IEnumerable<T> compare)
        {
            if (list.Count() < compare.Count())
            {
                throw new InvalidOperationException("ƒŠƒXƒg‚Ì’·‚³‚ª’Z‚·‚¬‚Ü‚·");
            }

            foreach(var element in compare)
            {
                if (!list.Any(x => x.Equals(element)))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Compare‚Ì—v‘f‚ğˆê‚Â‚Å‚àlist‚ª‚Á‚Ä‚¢‚é‚©
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="compare"></param>
        /// <returns></returns>
        public static bool AnyParts<T>(this IEnumerable<T> list, IEnumerable<T> compare)
        {
            if (list.Any(e => compare.Any(t => e.Equals(t))))
            {
                return true;
            }

            return false;
        }

        public static IEnumerable<IEnumerable<T>> IntersectForDoubleArray<T>(this IEnumerable<IEnumerable<T>> list, IEnumerable<IEnumerable<T>> compare)
        {
            foreach(var e in list)
            {
                foreach(var t in compare)
                {
                    var count_1 = e.Count();
                    var count_2 = e.Count();

                    if (count_1 != count_2)
                    {
                        continue;
                    }

                    int i = 0;
                    for(; i < count_1; i++)
                    {
                        if (!e.ElementAt(i).Equals(t.ElementAt(i)))
                        {
                            break;
                        }
                    }

                    if (i == count_1)
                    {
                        yield return e;
                    }
                }
            }
        }
    }
}

