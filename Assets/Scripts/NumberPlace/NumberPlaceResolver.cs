using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using System;

namespace NumberPlace
{
    public class NumberPlaceResolver : MonoBehaviour
    {
        [SerializeField] private QuestionManager questionManager;
        [SerializeField] private SquareManager squareManager;

        private readonly IEnumerable<int> numberList = Enumerable.Range(1, 9);

        private List<EmptyPoint> emptyPointList = new List<EmptyPoint>();

        private int[,][,] answer = new int[3, 3][,];

        private class EmptyPoint
        {
            public int x;
            public int y;
            public int v;
            public int h;
            public int[] usableNumberArray;

            public EmptyPoint(int x, int y, int v, int h, int[] usableNumberArray)
            {
                this.x = x;
                this.y = y;
                this.v = v;
                this.h = h;
                this.usableNumberArray = usableNumberArray;
            }
        }

        private void InitAnswerArray()
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    answer[y, x] = new int[3, 3];

                    for (int v = 0; v < 3; v++)
                    {
                        for (int h = 0; h < 3; h++)
                        {
                            int number = questionManager.GetNumber(x, y, h, v);
                            answer[y, x][v, h] = number;
                        }
                    }
                }
            }
        }

        public async UniTask<bool> Resolve()
        {
            InitAnswerArray();

            while (true)
            {
                emptyPointList.Clear();

                SearchEmptyPoints();

                await UniTask.Yield(this.GetCancellationTokenOnDestroy());

                if (emptyPointList.Count == 0)
                {
                    break;
                }

                if (FillConfiredPoints())
                    continue;

                if (TryFillVerticalPoints())
                    continue;

                if (TryFillHolizontalPoints())
                    continue;

                if (TryFillBlockPoints())
                    continue;

                //Debug.Log("muri");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 空欄の場所と当てはめが可能な数字を取得
        /// </summary>
        private void SearchEmptyPoints()
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    for (int v = 0; v < 3; v++)
                    {
                        for (int h = 0; h < 3; h++)
                        {
                            if (answer[y, x][v, h] == 0)
                            {
                                var usableNumberArray = GetUsableNumbers(x, y, v, h);
                                emptyPointList.Add(new EmptyPoint(x, y, v, h, usableNumberArray));
                            }
                        }
                    }
                }
            }
        }

        //指定したマスに選択可能な数字の配列を取得する
        private int[] GetUsableNumbers(int x, int y, int v, int h)
        {
            int baseX = x;
            int baseY = y;
            int baseV = v;
            int baseH = h;

            List<int> usedNumbers = new List<int>();

            for (x = 0; x < 3; x++)
            {
                for (y = 0; y < 3; y++)
                {
                    if(x != baseX && y != baseY)
                    {
                        continue;
                    }

                    for (h = 0; h < 3; h++)
                    {
                        for (v = 0; v < 3; v++)
                        {
                            //同じブロック内にある数字は使えない
                            if (x == baseX && y == baseY)
                            {
                                usedNumbers.Add(answer[y, x][v, h]);
                            }
                            else if(x == baseX && y != baseY)
                            {
                                //縦列にある数字は使えない
                                if (h == baseH)
                                {
                                    usedNumbers.Add(answer[y, x][v, h]);
                                }
                            }
                            else if(x != baseX && y == baseY)
                            {
                                //横列にある数字は使えない
                                if (v == baseV)
                                {
                                    usedNumbers.Add(answer[y, x][v, h]);
                                }
                            }
                        }
                    }
                }
            }

            return numberList.Except(usedNumbers).ToArray();
        }

        /// <summary>
        /// 確定している空欄を埋める
        /// </summary>
        /// <returns></returns>
        private bool FillConfiredPoints()
        {
            bool result = false;

            //選択できる数字が一つしかないなら確定
            var confiredEmptyPoint = emptyPointList.Where(x => x.usableNumberArray.Count() == 1).ToList();
            if (confiredEmptyPoint.Any())
            {
                foreach (var point in confiredEmptyPoint)
                {
                    answer[point.y, point.x][point.v, point.h] = point.usableNumberArray[0];
                    emptyPointList.Remove(point);

                    //squareManager.GetSquare(point.x, point.y, point.h, point.v).WriteNumber(point.usableNumberArray[0]);
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// 縦列で確定している空欄を埋める
        /// </summary>
        /// <returns></returns>
        private bool TryFillVerticalPoints()
        {
            //縦で確定しているマスを埋める
            for (int x = 0; x < 3; x++)
            {
                for (int h = 0; h < 3; h++)
                {
                    //同じ縦列の空欄を取得する
                    var verticalEmptyPointList = emptyPointList.Where(point => point.x == x && point.h == h).ToList();
                    if (verticalEmptyPointList.Count <= 2)
                    {
                        continue;
                    }

                    //各空欄の中で、ある数字が使える空欄が一つしかないなら確定
                    for(int n = 1; n <= 9; n++)
                    {
                        var points = verticalEmptyPointList.Where(p => p.usableNumberArray.Any(numberList => numberList == n)).ToList();
                        if (points.Count == 1)
                        {
                            answer[points[0].y, points[0].x][points[0].v, points[0].h] = n;
                            emptyPointList.Remove(points[0]);
                            return true;
                        }
                    }

                    //列の中で、ある空欄の使用可能な数字(numbers)が、同じ空欄が数字の数と同じだけあり、
                    //使用可能な数字からnumbersを除いた数字の数が一つしかないなら確定する
                    foreach(var numbers in verticalEmptyPointList.Select(point=>point.usableNumberArray))
                    {
                        var list = verticalEmptyPointList.Where(p => p.usableNumberArray.SequenceEqual(numbers)).ToList();
                        if (list.Count == numbers.Count())
                        {
                            list = verticalEmptyPointList.Where(p => p.usableNumberArray.Except(numbers).Count() == 1).ToList();
                            if (list.Any())
                            {
                                var n = list[0].usableNumberArray.Except(numbers).First();
                                answer[list[0].y, list[0].x][list[0].v, list[0].h] = n;
                                emptyPointList.Remove(list[0]);

                                //squareManager.GetSquare(list[0].x, list[0].y, list[0].h, list[0].v).WriteNumber(n);
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 横列で確定している空欄を埋める
        /// </summary>
        /// <returns></returns>
        private bool TryFillHolizontalPoints()
        {
            //横で確定しているマスを埋める
            for (int y = 0; y < 3; y++)
            {
                for (int v = 0; v < 3; v++)
                {
                    //同じ横列の空欄を取得する
                    var holizontalEmptyPointList = emptyPointList.Where(point => point.y == y && point.v == v).ToList();
                    if (holizontalEmptyPointList.Count <= 2)
                    {
                        continue;
                    }

                    //各空欄の中で、ある数字が使える空欄が一つしかないなら確定
                    for (int n = 1; n <= 9; n++)
                    {
                        var points = holizontalEmptyPointList.Where(p => p.usableNumberArray.Any(numberList => numberList == n)).ToList();
                        if (points.Count == 1)
                        {
                            answer[points[0].y, points[0].x][points[0].v, points[0].h] = n;
                            emptyPointList.Remove(points[0]);
                            return true;
                        }
                    }

                    //列の中で、ある空欄の使用可能な数字(numbers)が、同じ空欄が数字の数と同じだけあり、
                    //使用可能な数字からnumbersを除いた数字の数が一つしかないなら確定する
                    foreach (var numbers in holizontalEmptyPointList.Select(point => point.usableNumberArray))
                    {
                        var list = holizontalEmptyPointList.Where(p => p.usableNumberArray.SequenceEqual(numbers)).ToList();
                        if (list.Count == numbers.Count())
                        {
                            list = holizontalEmptyPointList.Where(p => p.usableNumberArray.Except(numbers).Count() == 1).ToList();
                            if (list.Any())
                            {
                                var n = list[0].usableNumberArray.Except(numbers).First();
                                answer[list[0].y, list[0].x][list[0].v, list[0].h] = n;
                                emptyPointList.Remove(list[0]);

                                //squareManager.GetSquare(list[0].x, list[0].y, list[0].h, list[0].v).WriteNumber(n);
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private bool TryFillBlockPoints()
        {
            //ブロックで確定しているマスを埋める
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    //同じブロックの空欄を取得する
                    var blockEmptyPointList = emptyPointList.Where(point => point.x == x && point.y == y).ToList();
                    if (blockEmptyPointList.Count <= 2)
                    {
                        continue;
                    }

                    //各空欄の中で、ある数字が使える空欄が一つしかないなら確定
                    for (int n = 1; n <= 9; n++)
                    {
                        var points = blockEmptyPointList.Where(p => p.usableNumberArray.Any(numberList => numberList == n)).ToList();
                        if (points.Count == 1)
                        {
                            answer[points[0].y, points[0].x][points[0].v, points[0].h] = n;
                            emptyPointList.Remove(points[0]);
                            return true;
                        }
                    }

                    //列の中で、ある空欄の使用可能な数字(numbers)が、同じ空欄が数字の数と同じだけあり、
                    //使用可能な数字からnumbersを除いた数字の数が一つしかないなら確定する
                    foreach (var numbers in blockEmptyPointList.Select(point => point.usableNumberArray))
                    {
                        var list = blockEmptyPointList.Where(p => p.usableNumberArray.SequenceEqual(numbers)).ToList();
                        if (list.Count == numbers.Count())
                        {
                            list = blockEmptyPointList.Where(p => p.usableNumberArray.Except(numbers).Count() == 1).ToList();
                            if (list.Any())
                            {
                                var n = list[0].usableNumberArray.Except(numbers).First();
                                answer[list[0].y, list[0].x][list[0].v, list[0].h] = n;
                                emptyPointList.Remove(list[0]);

                                //squareManager.GetSquare(list[0].x, list[0].y, list[0].h, list[0].v).WriteNumber(n);
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}

