using System;

namespace CSharpAdvanced.Topics
{
    public static class SearchingSorting
    {
        public static void Run()
        {
            int[] arr = { 5, 2, 9, 1, 5 };

            // Linear search
            int index = LinearSearch(arr, 9);
            Console.WriteLine("Linear Search index of 9: " + index);

            // Bubble sort
            BubbleSort(arr);
            Console.WriteLine("Bubble Sorted: " + string.Join(",", arr));

            // Selection sort
            int[] arr2 = { 5, 2, 9, 1, 5 };
            SelectionSort(arr2);
            Console.WriteLine("Selection Sorted: " + string.Join(",", arr2));

            // Insertion sort
            int[] arr3 = { 5, 2, 9, 1, 5 };
            InsertionSort(arr3);
            Console.WriteLine("Insertion Sorted: " + string.Join(",", arr3));
        }

        static int LinearSearch(int[] arr, int key)
        {
            for (int i = 0; i < arr.Length; i++)
                if (arr[i] == key) return i;
            return -1;
        }

        static void BubbleSort(int[] arr)
        {
            for (int i = 0; i < arr.Length - 1; i++)
                for (int j = 0; j < arr.Length - i - 1; j++)
                    if (arr[j] > arr[j + 1])
                    {
                        int temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                    }
        }

        static void SelectionSort(int[] arr)
        {
            for (int i = 0; i < arr.Length - 1; i++)
            {
                int minIdx = i;
                for (int j = i + 1; j < arr.Length; j++)
                    if (arr[j] < arr[minIdx])
                        minIdx = j;
                int temp = arr[i];
                arr[i] = arr[minIdx];
                arr[minIdx] = temp;
            }
        }

        static void InsertionSort(int[] arr)
        {
            for (int i = 1; i < arr.Length; i++)
            {
                int key = arr[i];
                int j = i - 1;
                while (j >= 0 && arr[j] > key)
                {
                    arr[j + 1] = arr[j];
                    j--;
                }
                arr[j + 1] = key;
            }
        }
    }
}
