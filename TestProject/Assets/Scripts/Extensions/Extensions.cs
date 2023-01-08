using System;
using System.Collections.Generic;

namespace Extensions
{
    public static class Extensions
    {
        public static void ClearOptimized<T>(this IList<T> colllection) where T : class
        {
            for (int i = 0; i < colllection.Count; i++)
            {
                colllection[i] = null;
            }
        }

        public static void SortSpan<T>(this ref Span<T> collection, Comparison<T> comparison)
            where T : unmanaged, IComparable<T>
        {
            Sort(ref collection, 0, collection.Length - 1, comparison);
        }

        private static void Sort<T>(ref Span<T> arr, int l, int r, Comparison<T> comparison)
            where T : unmanaged, IComparable<T>
        {
            if (l < r)
            {
                int m = l + (r - l) / 2;

                Sort(ref arr, l, m, comparison);
                Sort(ref arr, m + 1, r, comparison);
                Merge(ref arr, l, m, r, comparison);
            }
        }

        private static void Merge<T>(ref Span<T> arr, int l, int m, int r, Comparison<T> comparison)
            where T : unmanaged, IComparable<T>
        {
            int n1 = m - l + 1;
            int n2 = r - m;

            Span<T> L = stackalloc T[n1];
            Span<T> R = stackalloc T[n2];
            int i, j;

            for (i = 0; i < n1; ++i)
                L[i] = arr[l + i];
            for (j = 0; j < n2; ++j)
                R[j] = arr[m + 1 + j];

            i = 0;
            j = 0;

            int k = l;
            while (i < n1 && j < n2)
            {
                if (comparison.Invoke(L[i], R[j]) < 0)
                {
                    arr[k] = L[i];
                    i++;
                }
                else
                {
                    arr[k] = R[j];
                    j++;
                }

                k++;
            }

            while (i < n1)
            {
                arr[k] = L[i];
                i++;
                k++;
            }

            while (j < n2)
            {
                arr[k] = R[j];
                j++;
                k++;
            }
        }
    }
}