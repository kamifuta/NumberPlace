using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MyUtil;
using System;

public class NumberGenerator
{
    public int[,][,] GenerateNumbers()
    {
        int[,][,] numberArray = new int[3,3][,];
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                numberArray[i, j] = new int[3, 3] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
                (int, int)[] taples = new (int, int)[9]
                {
                        (0,0), (0,1), (0,2), (1,0), (1,1), (1,2), (2,0), (2,1), (2,2)
                };

                if (i == 0 && j == 0)
                {
                    for (int n = 1; n <= 9; n++)
                    {
                        var taple = taples.RandomGet();
                        numberArray[i, j][taple.Item1, taple.Item2] = n;
                        taples = taples.Where(x => x != taple).ToArray();
                    }
                }
                else if (i == 0)
                {
                    for (int n = 1; n <= 9; n++)
                    {
                        int[] vArray = new int[j];
                        for(int m = 0; m < j; m++)
                        {
                            var array = numberArray[i, m];
                            int t = 0;
                            foreach(var num in array)
                            {
                                if (n == num)
                                {
                                    var v = t / 3;
                                    vArray[m] = v;
                                    break;
                                }
                                t++;
                            }
                        }

                        var tmpTaples = taples.Where(x => vArray.Any(v => v != x.Item1)).ToArray();
                        var taple = tmpTaples.RandomGet();
                        numberArray[i, j][taple.Item1, taple.Item2] = n;
                        taples = taples.Where(x => x != taple).ToArray();
                    }
                }
                else if (j == 0)
                {
                    for (int n = 1; n <= 9; n++)
                    {
                        int[] hArray = new int[i];
                        for (int m = 0; m < i; m++)
                        {
                            var array = numberArray[m, j];
                            int t = 0;
                            foreach (var num in array)
                            {
                                if (n == num)
                                {
                                    var h = t % 3;
                                    hArray[m] = h;
                                    break;
                                }
                                t++;
                            }
                        }

                        var tmpTaples = taples.Where(x => hArray.Any(h => h != x.Item2)).ToArray();
                        var taple = tmpTaples.RandomGet();
                        numberArray[i, j][taple.Item1, taple.Item2] = n;
                        taples = taples.Where(x => x != taple).ToArray();
                    }
                }
                else
                {
                    for (int n = 1; n <= 9; n++)
                    {
                        for (int m = 0; m < i; m++)
                        {
                            for(int s = 0; s < j; s++)
                            {
                                var array = numberArray[m, s];
                                int t = 0;
                                foreach (var num in array)
                                {
                                    if (t == num)
                                    {
                                        var v = t / 3;
                                        var h = t % 3;
                                        var tmpTaples = taples.Where(x => x.Item1 != v && x.Item2 != h).ToArray();
                                        var taple = tmpTaples.RandomGet();
                                        numberArray[i, j][taple.Item1, taple.Item2] = n;
                                        taples = taples.Where(x => x != taple).ToArray();
                                        break;
                                    }
                                    t++;
                                }
                                if (t != array.Length) break;
                            }
                        }
                    }
                }
            }
        }
        return numberArray;
    }
}
