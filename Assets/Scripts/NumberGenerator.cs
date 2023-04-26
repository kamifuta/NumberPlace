using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MyUtil;
using System;

public class NumberGenerator
{
    private int[,][,] numberArray;
    private readonly int[] numbers = new int[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

    public int[,][,] GenerateNumbers()
    {
        numberArray = new int[3,3][,];

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                numberArray[i, j] = new int[3, 3] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
            }
        }

        numberArray[0, 0][0, 0] = numbers.RandomGet();

        try
        {
            for (int m = 1; m < 17; m++)
            {
                int i, j, k, l;
                int[] usableNumbers;

                for (int n = 0; n < m; n++)
                {
                    i = m / 3;
                    j = n / 3;
                    k = m - i * 3;
                    l = n - j * 3;
                    usableNumbers = GetUsableNumbers(i, j, k, l);
                    numberArray[i, j][k, l] = usableNumbers.RandomGet();

                    i = n / 3;
                    j = m / 3;
                    k = n - i * 3;
                    l = m - j * 3;
                    usableNumbers = GetUsableNumbers(i, j, k, l);
                    numberArray[i, j][k, l] = usableNumbers.RandomGet();
                }

                i = m / 3;
                j = m / 3;
                k = m - i * 3;
                l = m - j * 3;
                usableNumbers = GetUsableNumbers(i, j, k, l);
                numberArray[i, j][k, l] = usableNumbers.RandomGet();
            }
        }
        catch (ArgumentOutOfRangeException)
        {

        }
        

        //for (int i = 0; i < 3; i++)
        //{
        //    for (int j = 0; j < 3; j++)
        //    {
        //        //‚±‚±‚É–ß‚é
        //        try
        //        {
        //            for (int k = 0; k < 3; k++)
        //            {
        //                for (int l = 0; l < 3; l++)
        //                {
        //                    var usableNumbers = GetUsableNumbers(i, j, k, l);
        //                    var number = usableNumbers.RandomGet();
        //                    numberArray[i, j][k, l] = number;
        //                }
        //            }
        //        }
        //        catch (ArgumentOutOfRangeException)
        //        {
        //            numberArray[i, j] = new int[3, 3] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
        //            j--;
        //        }
        //    }
        //}



        //numberArray[0, 0] = GenerateFreeNumbers();
        //numberArray[1, 1] = GenerateFreeNumbers();
        //numberArray[2, 2] = GenerateFreeNumbers();

        //GenerateBlockNumbers(0, 2);
        //GenerateBlockNumbers(2, 0);

        //GenerateBlockNumbers(0, 1);
        //GenerateBlockNumbers(2, 1);
        //GenerateBlockNumbers(1, 0);
        //GenerateBlockNumbers(1, 2);

        return numberArray;
    }

    private int[,] GenerateFreeNumbers()
    {
        var array= new int[3, 3] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
        (int, int)[] taples = new (int, int)[9]
        {
            (0,0), (0,1), (0,2), (1,0), (1,1), (1,2), (2,0), (2,1), (2,2)
        };

        for (int n = 1; n <= 9; n++)
        {
            var taple = taples.RandomGet();
            array[taple.Item1, taple.Item2] = n;
            taples = taples.Where(x => x != taple).ToArray();
        }
        return array;
    }

    private void GenerateBlockNumbers(int i, int j)
    {
        numberArray[i, j] = new int[3, 3] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
        (int, int)[] taples = new (int, int)[9]
        {
                    (0,0), (0,1), (0,2), (1,0), (1,1), (1,2), (2,0), (2,1), (2,2)
        };

        for (int n = 1; n <= 9; n++)
        {
            List<int> vList = new List<int>();
            List<int> hList = new List<int>();

            for (int k = -2; k <= 2; k++)
            {
                if (i + k < 0 || 2 < i + k) continue;

                for (int l = -2; l <= 2; l++)
                {
                    if (k == 0 && l == 0) continue;
                    if (k != 0 && l != 0) continue;

                    if (j + l < 0 || 2 < j + l) continue;

                    if (numberArray[i + k, j + l] == null) continue;

                    var array = numberArray[i + k, j + l];
                    int index = 0;
                    foreach (var num in array)
                    {
                        if (n == num)
                        {
                            if (k == 0)
                            {
                                vList.Add(index / 3);
                                break;
                            }
                            else if (l == 0)
                            {
                                hList.Add(index % 3);
                                break;
                            }
                        }
                        index++;
                    }
                }
            }

            var tmpTaples = taples.Where(x => !vList.Any(v => v == x.Item1) && !hList.Any(h => h == x.Item2)).ToArray();
            var taple = tmpTaples.RandomGet();
            numberArray[i, j][taple.Item1, taple.Item2] = n;
            taples = taples.Where(x => x != taple).ToArray();
        }
    }

    private int[] GetUsableNumbers(int i, int j, int k, int l)
    {
        var hashSet = new HashSet<int>();
        
        foreach(var n in numberArray[i, j])
        {
            hashSet.Add(n);
        }

        for (int y = -2; y <= 2; y++)
        {
            if (i + y < 0 || 2 < i + y) continue;

            for (int x = -2; x <= 2; x++)
            {
                if (!(y == 0 ^ x == 0)) continue;

                if (j + x < 0 || 2 < j + x) continue;

                if (y == 0)
                {
                    hashSet.Add(numberArray[i, j + x][k, 0]);
                    hashSet.Add(numberArray[i, j + x][k, 1]);
                    hashSet.Add(numberArray[i, j + x][k, 2]);
                }
                else if (x == 0)
                {
                    hashSet.Add(numberArray[i + y, j][0, l]);
                    hashSet.Add(numberArray[i + y, j][1, l]);
                    hashSet.Add(numberArray[i + y, j][2, l]);
                }
            }
        }

        return numbers.Except(hashSet).ToArray();
    }
}
