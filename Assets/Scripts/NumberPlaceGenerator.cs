using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Kamifuta.MyUtil;
using System;
using Random = UnityEngine.Random;
using System.Threading;

namespace NumberPlace
{
    public class NumberPlaceGenerator : MonoBehaviour
    {
        private int[,][,] numberPlace = new int[3,3][,];

        private readonly IEnumerable<int> numberArray = Enumerable.Range(1, 9);

        private void Start()
        {
            InitNumberPlace();
            GenerateNumberPlace();
            
            Debug.Log("生成が完了しました");
        }

        private void InitNumberPlace()
        {
            for(int x = 0; x < 3; x++)
            {
                for(int y = 0; y < 3; y++)
                {
                    numberPlace[y, x] = new int[3, 3];

                    for (int v = 0; v < 3; v++)
                    {
                        for (int h = 0; h < 3; h++)
                        {
                            numberPlace[y, x][h, v] = 0;
                        }
                    }
                }
            }
        }

        private void GenerateNumberPlace()
        {
            AlineHorizintalNumberPlace();
            AlineVerticalNumberPlace();
        }

        /// <summary>
        /// 横方向とブロック内で1~9がひとつづつしか存在しないように数字を並べる
        /// </summary>
        private void AlineHorizintalNumberPlace()
        {
            List<(int block, IEnumerable<int> numberList)> setNumbersListOnBlock = new List<(int block, IEnumerable<int> numberList)>();
            List<(int line, IEnumerable<int> numberList)> setNumbersListOnLine = new List<(int line, IEnumerable<int> numberList)>();

            for(int y = 0; y < 3; y++)
            {
                for (int h = 0; h < 3; h++)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        int block = y * 3 + x;
                        int line = y * 3 + h;
                        IEnumerable<int> usedNumberList = setNumbersListOnBlock.Where(t => t.block == block).Concat(setNumbersListOnLine.Where(t => t.line == line)).SelectMany(t => t.numberList);
                        IEnumerable<int> usableNumbers = numberArray.Except(usedNumberList).ToArray();
                        IEnumerable<IEnumerable<int>> selectablePermutationList = usableNumbers.Permutation(3);
                        var permutation = selectablePermutationList.RandomGet();

                        if (x == 1)
                        {
                            IEnumerable<int> mustUseNumbers = setNumbersListOnBlock.Where(t => t.block == block + 1).SelectMany(t => t.numberList).Except(setNumbersListOnLine.Where(t => t.line == line).SelectMany(t => t.numberList));
                            selectablePermutationList = selectablePermutationList.Where(list => list.Any(mustUseNumbers));

                            permutation = selectablePermutationList.RandomGet();
                        }

                        setNumbersListOnBlock.Add((block, permutation));
                        setNumbersListOnLine.Add((line, permutation));

                        for (int v = 0; v < 3; v++)
                        {
                            numberPlace[y, x][h, v] = permutation.ElementAt(v);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 縦方向で1~9がひとつづつしか存在しないように数字を並べ替える
        /// </summary>
        private void AlineVerticalNumberPlace()
        {
            for (int x = 0; x < 3; x++)
            {
                List<IEnumerable<IEnumerable<int>>> settableNumbersArray = new List<IEnumerable<IEnumerable<int>>>();

                for (int y = 0; y < 3; y++)
                {
                    int[][] doubleArray = new int[3][];

                    for (int h = 0; h < 3; h++)
                    {
                        int[] array = new int[3];

                        for (int v = 0; v < 3; v++)
                        {
                            array[v] = numberPlace[y, x][h, v];
                        }

                        doubleArray[h] = array;
                    }

                    var list = PermutationForDoubleList(doubleArray).ToArray();
                    settableNumbersArray.Add(list);
                }

                IEnumerable<int>[] upperNumberList = new IEnumerable<int>[3] { Enumerable.Empty<int>(), Enumerable.Empty<int>(), Enumerable.Empty<int>() };
                IEnumerable<int>[] middleNumberList = new IEnumerable<int>[3] { Enumerable.Empty<int>(), Enumerable.Empty<int>(), Enumerable.Empty<int>() };
                IEnumerable<int>[] bottomNumberList = new IEnumerable<int>[3] { Enumerable.Empty<int>(), Enumerable.Empty<int>(), Enumerable.Empty<int>() };

                for (int v = 0; v < 3; v++)
                {
                    List<IEnumerable<int>> tryNumberList = new List<IEnumerable<int>>();

                    
                    IEnumerable<IEnumerable<int>> upperSettableNumberList;
                    IEnumerable<IEnumerable<int>> middleSettableNumberList;
                    IEnumerable<IEnumerable<int>> bottomSettableNumberList;
                    IEnumerable<int> usableNumbers;

                    while (true)
                    {
                        usableNumbers = numberArray.Except(upperNumberList.SelectMany(x => x)).ToList();
                        upperSettableNumberList = settableNumbersArray[0].Except(tryNumberList).IntersectForDoubleArray(usableNumbers.Permutation(3)).ToList();
                        if (!upperSettableNumberList.Any())
                        {
                            tryNumberList.Add(upperNumberList[v]);
                            continue;
                        }
                        else
                        {
                            upperNumberList[v] = upperSettableNumberList.RandomGet();
                        }
                        

                        usableNumbers= numberArray.Except(middleNumberList.SelectMany(x => x)).Except(upperNumberList[v]).ToList();
                        middleSettableNumberList = settableNumbersArray[1].IntersectForDoubleArray(usableNumbers.Permutation(3)).ToList();
                        if (!middleSettableNumberList.Any())
                        {
                            upperNumberList[v] = Enumerable.Empty<int>();
                            tryNumberList.Add(upperNumberList[v]);
                            continue;
                        }
                        else
                        {
                            middleNumberList[v] = middleSettableNumberList.RandomGet();
                        }

                        usableNumbers = numberArray.Except(bottomNumberList.SelectMany(x => x)).Except(upperNumberList[v]).Except(middleNumberList[v]).ToList();
                        if (usableNumbers.Count() < 3)
                        {
                            upperNumberList[v] = Enumerable.Empty<int>();
                            middleNumberList[v] = Enumerable.Empty<int>();
                            tryNumberList.Add(upperNumberList[v]);
                            continue;
                        }

                        bottomSettableNumberList = settableNumbersArray[2].IntersectForDoubleArray(usableNumbers.Permutation(3)).ToList();

                        if (!bottomSettableNumberList.Any())
                        {
                            upperNumberList[v] = Enumerable.Empty<int>();
                            middleNumberList[v] = Enumerable.Empty<int>();
                            tryNumberList.Add(upperNumberList[v]);
                        }
                        else
                        {
                            bottomNumberList[v] = bottomSettableNumberList.RandomGet();
                            break;
                        }
                    }

                    tryNumberList.Clear();
                }

                for (int v = 0; v < 3; v++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        List<int> setNumberList = new List<int>();
                        switch (y)
                        {
                            case 0:
                                setNumberList = upperNumberList[v].ToList();
                                break;
                            case 1:
                                setNumberList = middleNumberList[v].ToList();
                                break;
                            case 2:
                                setNumberList = bottomNumberList[v].ToList();
                                break;
                        }

                        for (int h = 0; h < 3; h++)
                        {
                            numberPlace[y, x][h, v] = setNumberList[h];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 複数のリストからそれぞれの要素を一つずつ含むリストを返す
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        private IEnumerable<IEnumerable<T>> PermutationForDoubleList<T>(IEnumerable<IEnumerable<T>> list)
        {
            if (list.Count() == 1)
            {
                foreach(var element in list.ElementAt(0))
                {
                    yield return new T[] { element };
                }
            }
            else
            {
                var left = list.ElementAt(0);

                foreach(var elementList in PermutationForDoubleList(list.Skip(1)))
                {
                    foreach(var element in left)
                    {
                        yield return new T[] { element }.Concat(elementList).ToArray();
                    }
                }
            }
        }
    }
}

