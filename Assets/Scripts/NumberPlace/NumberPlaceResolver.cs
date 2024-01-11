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

                Debug.Log("muri");
                return false;
            }

            return true;
        }

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
                            if (x == baseX && y == baseY)
                            {
                                usedNumbers.Add(answer[y, x][v, h]);
                            }
                            else if(x == baseX && y != baseY)
                            {
                                if (h == baseH)
                                {
                                    usedNumbers.Add(answer[y, x][v, h]);
                                }
                            }
                            else if(x != baseX && y == baseY)
                            {
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

        private bool FillConfiredPoints()
        {
            bool result = false;

            //確定しているマスを埋める
            var confiredEmptyPoint = emptyPointList.Where(x => x.usableNumberArray.Count() == 1).ToList();
            if (confiredEmptyPoint.Any())
            {
                foreach (var point in confiredEmptyPoint)
                {
                    answer[point.y, point.x][point.v, point.h] = point.usableNumberArray[0];
                    emptyPointList.Remove(point);

                    squareManager.GetSquare(point.x, point.y, point.h, point.v).WriteNumber(point.usableNumberArray[0]);
                    result = true;
                }
            }

            return result;
        }

        private bool TryFillVerticalPoints()
        {
            //縦で確定しているマスを埋める
            for (int x = 0; x < 3; x++)
            {
                for (int h = 0; h < 3; h++)
                {
                    var verticalEmptyPointList = emptyPointList.Where(point => point.x == x && point.h == h).ToList();
                    if (verticalEmptyPointList.Count <= 2)
                    {
                        continue;
                    }

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

                                squareManager.GetSquare(list[0].x, list[0].y, list[0].h, list[0].v).WriteNumber(n);
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private bool TryFillHolizontalPoints()
        {
            //横で確定しているマスを埋める
            for (int y = 0; y < 3; y++)
            {
                for (int v = 0; v < 3; v++)
                {
                    var holizontalEmptyPointList = emptyPointList.Where(point => point.y == y && point.v == v).ToList();
                    if (holizontalEmptyPointList.Count <= 2)
                    {
                        continue;
                    }

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

                                squareManager.GetSquare(list[0].x, list[0].y, list[0].h, list[0].v).WriteNumber(n);
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
                    var blockEmptyPointList = emptyPointList.Where(point => point.x == x && point.y == y).ToList();
                    if (blockEmptyPointList.Count <= 2)
                    {
                        continue;
                    }

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

                                squareManager.GetSquare(list[0].x, list[0].y, list[0].h, list[0].v).WriteNumber(n);
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

