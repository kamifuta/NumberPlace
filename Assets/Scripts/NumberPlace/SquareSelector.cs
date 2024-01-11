using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Linq;

namespace NumberPlace
{
    public class SquareSelector : MonoBehaviour
    {
        [SerializeField] private SquareManager squareManager;
        [SerializeField] private QuestionManager questionManager;

        public Square selectedSquare { get; private set; }
        private IEnumerable<Square> emphasizedSquares = Enumerable.Empty<Square>();

        public void SetSelectedAction()
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    for (int h = 0; h < 3; h++)
                    {
                        for (int v = 0; v < 3; v++)
                        {
                            var square = squareManager.GetSquare(x, y, h, v);

                            int squareX = x;
                            int squareY = y;
                            int squareH = h;
                            int squareV = v;

                            square.OnMouseDownAsObservable()
                                .Subscribe(_ =>
                                {
                                    selectedSquare?.SetSelected(false);
                                    foreach(var emphasizedSquare in emphasizedSquares)
                                    {
                                        emphasizedSquare.SetEmphasis(false);
                                    }

                                    selectedSquare = square;
                                    emphasizedSquares = GetEmphasizeSquares(questionManager.GetNumber(squareX, squareY, squareH, squareV));

                                    foreach (var emphasizedSquare in emphasizedSquares)
                                    {
                                        emphasizedSquare.SetEmphasis(true);
                                    }
                                    square.SetSelected(true);
                                })
                                .AddTo(this);
                        }
                    }
                }
            }
        }

        private IEnumerable<Square> GetEmphasizeSquares(int n)
        {
            List<Square> resultList = new List<Square>();

            if (n == 0)
            {
                return resultList;
            }

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    for (int h = 0; h < 3; h++)
                    {
                        for (int v = 0; v < 3; v++)
                        {
                            var number = questionManager.GetNumber(x, y, h, v);
                            if (number == n)
                            {
                                resultList.Add(squareManager.GetSquare(x, y, h, v));
                            }
                        }
                    }
                }
            }

            return resultList;
        }
    }
}

