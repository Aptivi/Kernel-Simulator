﻿//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;

namespace Nitrocid.Drivers.Sorting.Bases
{
    internal class QuickSorting : BaseSortingDriver, ISortingDriver
    {
        /// <inheritdoc/>
        public override string DriverName =>
            "Quick";

        /// <inheritdoc/>
        public override byte[] SortNumbersInt8(byte[] unsorted)
        {
            // Implementation
            static byte[] QuickSort(byte[] target, int left, int right)
            {
                // Get the number of iterations
                int start = left;
                int end = right;
                var pivot = target[start];

                // Now, iterate through the whole array to check to see if we need to sort or not
                while (start <= end)
                {
                    while (target[start] < pivot)
                        start++;
                    while (target[end] > pivot)
                        end--;
                    if (start <= end)
                    {
                        (target[start], target[end]) = (target[end], target[start]);
                        start++;
                        end--;
                    }
                }

                // Sort the subarrays
                if (left < end)
                    QuickSort(target, left, end);
                if (start < right)
                    QuickSort(target, start, right);
                return target;
            }

            // Now, sort the array
            unsorted = QuickSort(unsorted, 0, unsorted.Length - 1);
            return unsorted;
        }

        /// <inheritdoc/>
        public override short[] SortNumbersInt16(short[] unsorted)
        {
            // Implementation
            static short[] QuickSort(short[] target, int left, int right)
            {
                // Get the number of iterations
                int start = left;
                int end = right;
                var pivot = target[start];

                // Now, iterate through the whole array to check to see if we need to sort or not
                while (start <= end)
                {
                    while (target[start] < pivot)
                        start++;
                    while (target[end] > pivot)
                        end--;
                    if (start <= end)
                    {
                        (target[start], target[end]) = (target[end], target[start]);
                        start++;
                        end--;
                    }
                }

                // Sort the subarrays
                if (left < end)
                    QuickSort(target, left, end);
                if (start < right)
                    QuickSort(target, start, right);
                return target;
            }

            // Now, sort the array
            unsorted = QuickSort(unsorted, 0, unsorted.Length - 1);
            return unsorted;
        }

        /// <inheritdoc/>
        public override int[] SortNumbersInt32(int[] unsorted)
        {
            // Implementation
            static int[] QuickSort(int[] target, int left, int right)
            {
                // Get the number of iterations
                int start = left;
                int end = right;
                var pivot = target[start];

                // Now, iterate through the whole array to check to see if we need to sort or not
                while (start <= end)
                {
                    while (target[start] < pivot)
                        start++;
                    while (target[end] > pivot)
                        end--;
                    if (start <= end)
                    {
                        (target[start], target[end]) = (target[end], target[start]);
                        start++;
                        end--;
                    }
                }

                // Sort the subarrays
                if (left < end)
                    QuickSort(target, left, end);
                if (start < right)
                    QuickSort(target, start, right);
                return target;
            }

            // Now, sort the array
            unsorted = QuickSort(unsorted, 0, unsorted.Length - 1);
            return unsorted;
        }

        /// <inheritdoc/>
        public override long[] SortNumbersInt64(long[] unsorted)
        {
            // Implementation
            static long[] QuickSort(long[] target, int left, int right)
            {
                // Get the number of iterations
                int start = left;
                int end = right;
                var pivot = target[start];

                // Now, iterate through the whole array to check to see if we need to sort or not
                while (start <= end)
                {
                    while (target[start] < pivot)
                        start++;
                    while (target[end] > pivot)
                        end--;
                    if (start <= end)
                    {
                        (target[start], target[end]) = (target[end], target[start]);
                        start++;
                        end--;
                    }
                }

                // Sort the subarrays
                if (left < end)
                    QuickSort(target, left, end);
                if (start < right)
                    QuickSort(target, start, right);
                return target;
            }

            // Now, sort the array
            unsorted = QuickSort(unsorted, 0, unsorted.Length - 1);
            return unsorted;
        }

        /// <inheritdoc/>
        public override Int128[] SortNumbersInt128(Int128[] unsorted)
        {
            // Implementation
            static Int128[] QuickSort(Int128[] target, int left, int right)
            {
                // Get the number of iterations
                int start = left;
                int end = right;
                var pivot = target[start];

                // Now, iterate through the whole array to check to see if we need to sort or not
                while (start <= end)
                {
                    while (target[start] < pivot)
                        start++;
                    while (target[end] > pivot)
                        end--;
                    if (start <= end)
                    {
                        (target[start], target[end]) = (target[end], target[start]);
                        start++;
                        end--;
                    }
                }

                // Sort the subarrays
                if (left < end)
                    QuickSort(target, left, end);
                if (start < right)
                    QuickSort(target, start, right);
                return target;
            }

            // Now, sort the array
            unsorted = QuickSort(unsorted, 0, unsorted.Length - 1);
            return unsorted;
        }

        /// <inheritdoc/>
        public override float[] SortNumbersFloat(float[] unsorted)
        {
            // Implementation
            static float[] QuickSort(float[] target, int left, int right)
            {
                // Get the number of iterations
                int start = left;
                int end = right;
                var pivot = target[start];

                // Now, iterate through the whole array to check to see if we need to sort or not
                while (start <= end)
                {
                    while (target[start] < pivot)
                        start++;
                    while (target[end] > pivot)
                        end--;
                    if (start <= end)
                    {
                        (target[start], target[end]) = (target[end], target[start]);
                        start++;
                        end--;
                    }
                }

                // Sort the subarrays
                if (left < end)
                    QuickSort(target, left, end);
                if (start < right)
                    QuickSort(target, start, right);
                return target;
            }

            // Now, sort the array
            unsorted = QuickSort(unsorted, 0, unsorted.Length - 1);
            return unsorted;
        }

        /// <inheritdoc/>
        public override double[] SortNumbersDouble(double[] unsorted)
        {
            // Implementation
            static double[] QuickSort(double[] target, int left, int right)
            {
                // Get the number of iterations
                int start = left;
                int end = right;
                var pivot = target[start];

                // Now, iterate through the whole array to check to see if we need to sort or not
                while (start <= end)
                {
                    while (target[start] < pivot)
                        start++;
                    while (target[end] > pivot)
                        end--;
                    if (start <= end)
                    {
                        (target[start], target[end]) = (target[end], target[start]);
                        start++;
                        end--;
                    }
                }

                // Sort the subarrays
                if (left < end)
                    QuickSort(target, left, end);
                if (start < right)
                    QuickSort(target, start, right);
                return target;
            }

            // Now, sort the array
            unsorted = QuickSort(unsorted, 0, unsorted.Length - 1);
            return unsorted;
        }

    }
}
