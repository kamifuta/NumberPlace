using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Kamifuta.MyUtil;

namespace NumberPlace
{
    public class NumberPlaceGenerator : MonoBehaviour
    {
        [SerializeField] private NumberPlaceManager numberPlaceManager;
        [SerializeField] private NumberPlaceViewer numberPlaceViewer;

        private readonly IEnumerable<int> numberArray = Enumerable.Range(1, 9);

        public void GenerateNumberPlace()
        {
            AlineHorizintalNumberPlace();
            AlineVerticalNumberPlace();
        }

        /// <summary>
        /// 横方向とブロック内で1~9がひとつづつしか存在しないように数字を並べる
        /// </summary>
        private void AlineHorizintalNumberPlace()
        {
            //ブロックと横ラインで使用された数字を格納するリスト
            List<(int block, IEnumerable<int> numberList)> setNumbersListOnBlock = new List<(int block, IEnumerable<int> numberList)>();
            List<(int line, IEnumerable<int> numberList)> setNumbersListOnLine = new List<(int line, IEnumerable<int> numberList)>();

            //ブロック内の横列を一列ずつ埋める
            for(int y = 0; y < 3; y++)
            {
                for (int v = 0; v < 3; v++)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        int block = y * 3 + x;
                        int line = y * 3 + v;
                        //横とブロックで使用されてる数字
                        IEnumerable<int> usedNumberList = setNumbersListOnBlock.Where(t => t.block == block).Concat(setNumbersListOnLine.Where(t => t.line == line)).SelectMany(t => t.numberList);
                        //使うことができる数字
                        IEnumerable<int> usableNumbers = numberArray.Except(usedNumberList).ToArray();
                        //セット可能な順列を取得する
                        IEnumerable<IEnumerable<int>> selectablePermutationList = usableNumbers.Permutation(3);
                        var permutation = selectablePermutationList.RandomGet();


                        //中央縦列ブロックの中央横列をセットするときだけ注意が必要
                        if (x == 1)
                        {
                            //使わなければならない数字を取得する
                            IEnumerable<int> mustUseNumbers = setNumbersListOnBlock.Where(t => t.block == block + 1).SelectMany(t => t.numberList).Except(setNumbersListOnLine.Where(t => t.line == line).SelectMany(t => t.numberList));
                            selectablePermutationList = selectablePermutationList.Where(list => list.Any(mustUseNumbers));

                            permutation = selectablePermutationList.RandomGet();
                        }

                        setNumbersListOnBlock.Add((block, permutation));
                        setNumbersListOnLine.Add((line, permutation));

                        //選択した数字をセット
                        for (int h = 0; h < 3; h++)
                        {
                            numberPlaceManager.SetNumber(x, y, h, v, permutation.ElementAt(h));
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
                //選択可能な順列を格納しておく
                List<IEnumerable<IEnumerable<int>>> settableNumbersArray = new List<IEnumerable<IEnumerable<int>>>();

                for (int y = 0; y < 3; y++)
                {
                    int[][] doubleArray = new int[3][];

                    for (int v = 0; v < 3; v++)
                    {
                        int[] array = new int[3];

                        for (int h = 0; h < 3; h++)
                        {
                            array[h] = numberPlaceManager.GetNumber(x, y, h, v);
                        }

                        doubleArray[v] = array;
                    }

                    var list = PermutationForDoubleList(doubleArray).ToArray();
                    settableNumbersArray.Add(list);
                }

                //ブロックごとの数字の順列を格納するリスト
                IEnumerable<int>[] upperNumberList = new IEnumerable<int>[3] { Enumerable.Empty<int>(), Enumerable.Empty<int>(), Enumerable.Empty<int>() };
                IEnumerable<int>[] middleNumberList = new IEnumerable<int>[3] { Enumerable.Empty<int>(), Enumerable.Empty<int>(), Enumerable.Empty<int>() };
                IEnumerable<int>[] bottomNumberList = new IEnumerable<int>[3] { Enumerable.Empty<int>(), Enumerable.Empty<int>(), Enumerable.Empty<int>() };

                for (int h = 0; h < 3; h++)
                {
                    //きれいに数字をセットできなかったときの順列を入れておく
                    List<IEnumerable<int>> tryNumberList = new List<IEnumerable<int>>();

                    //それぞれのブロックで選択可能な順列のリスト
                    IEnumerable<IEnumerable<int>> upperSettableNumberList;
                    IEnumerable<IEnumerable<int>> middleSettableNumberList;
                    IEnumerable<IEnumerable<int>> bottomSettableNumberList;
                    IEnumerable<int> usableNumbers;

                    while (true)
                    {
                        //上部のブロックで選択可能な順列の取得
                        usableNumbers = numberArray.Except(upperNumberList.SelectMany(x => x)).ToList();
                        upperSettableNumberList = settableNumbersArray[0].Except(tryNumberList).IntersectForDoubleArray(usableNumbers.Permutation(3)).ToList();
                        if (!upperSettableNumberList.Any())
                        {
                            tryNumberList.Add(upperNumberList[h]);
                            continue;
                        }
                        else
                        {
                            upperNumberList[h] = upperSettableNumberList.RandomGet();
                        }
                        
                        //中部のブロックで選択可能な順列の取得
                        usableNumbers= numberArray.Except(middleNumberList.SelectMany(x => x)).Except(upperNumberList[h]).ToList();
                        middleSettableNumberList = settableNumbersArray[1].IntersectForDoubleArray(usableNumbers.Permutation(3)).ToList();
                        if (!middleSettableNumberList.Any())
                        {
                            upperNumberList[h] = Enumerable.Empty<int>();
                            tryNumberList.Add(upperNumberList[h]);
                            continue;
                        }
                        else
                        {
                            middleNumberList[h] = middleSettableNumberList.RandomGet();
                        }

                        //下部のブロックで使用可能な数字の取得
                        usableNumbers = numberArray.Except(bottomNumberList.SelectMany(x => x)).Except(upperNumberList[h]).Except(middleNumberList[h]).ToList();
                        if (usableNumbers.Count() < 3)
                        {
                            upperNumberList[h] = Enumerable.Empty<int>();
                            middleNumberList[h] = Enumerable.Empty<int>();
                            tryNumberList.Add(upperNumberList[h]);
                            continue;
                        }

                        bottomSettableNumberList = settableNumbersArray[2].IntersectForDoubleArray(usableNumbers.Permutation(3)).ToList();

                        if (!bottomSettableNumberList.Any())
                        {
                            upperNumberList[h] = Enumerable.Empty<int>();
                            middleNumberList[h] = Enumerable.Empty<int>();
                            tryNumberList.Add(upperNumberList[h]);
                        }
                        else
                        {
                            bottomNumberList[h] = bottomSettableNumberList.RandomGet();
                            break;
                        }
                    }

                    tryNumberList.Clear();
                }

                //決められた数字をセットしていく
                for (int h = 0; h < 3; h++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        List<int> setNumberList = new List<int>();
                        switch (y)
                        {
                            case 0:
                                setNumberList = upperNumberList[h].ToList();
                                break;
                            case 1:
                                setNumberList = middleNumberList[h].ToList();
                                break;
                            case 2:
                                setNumberList = bottomNumberList[h].ToList();
                                break;
                        }

                        for (int v = 0; v < 3; v++)
                        {
                            numberPlaceManager.SetNumber(x, y, h, v, setNumberList[v]);
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

