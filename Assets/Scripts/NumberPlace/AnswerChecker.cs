using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace NumberPlace
{
    public class AnswerChecker : MonoBehaviour
    {
        [SerializeField] private NumberPlaceManager numberPlaceManager;
        [SerializeField] private SquareManager squareManager;

        [SerializeField] private Button checkButton;

        private readonly Subject<ResultType> resultSubject = new Subject<ResultType>();
        public IObservable<ResultType> resultObservable => resultSubject;

        private void Start()
        {
            checkButton.onClick.AddListener(() => CheckAnswer());
        }

        public void SetActiveCheckButton()
        {
            if (checkButton.interactable)
                return;

            for(int x = 0; x < 3; x++)
            {
                for(int y = 0; y < 3; y++)
                {
                    for(int h = 0; h < 3; h++)
                    {
                        for(int v = 0; v < 3; v++)
                        {
                            if (squareManager.GetSquare(x, y, h, v).NumberText == "")
                            {
                                checkButton.interactable = false;
                                return;
                            }
                        }
                    }
                }
            }

            checkButton.interactable = true;
        }

        private void CheckAnswer()
        {
            List<Square> misstakeSquareList = new List<Square>();

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    for (int h = 0; h < 3; h++)
                    {
                        for (int v = 0; v < 3; v++)
                        {
                            var square = squareManager.GetSquare(x, y, h, v);
                            if (square.NumberText != numberPlaceManager.GetNumber(x, y, h, v).ToString())
                            {
                                misstakeSquareList.Add(square);
                            }
                        }
                    }
                }
            }

            if (misstakeSquareList.Any())
            {
                ViewMisstake(misstakeSquareList);
                resultSubject.OnNext(ResultType.Failure);
            }
            else
            {
                resultSubject.OnNext(ResultType.Success);
            }

            checkButton.interactable = false;

            resultSubject.OnCompleted();
            resultSubject.Dispose();
        }

        private void ViewMisstake(IEnumerable<Square> misstaleSquareList)
        {
            foreach(var square in misstaleSquareList)
            {
                square.SetTextColor(Color.red);
            }
        }

        private void OnDestroy()
        {
            resultSubject?.Dispose();
        }
    }
}

