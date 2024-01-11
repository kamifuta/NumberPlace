using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx.Triggers;
using UniRx;
using System;

namespace NumberPlace
{
    public class SquareWriter : MonoBehaviour
    {
        [SerializeField] private SquareSelector squareSelector;
        [SerializeField] private AnswerChecker answerChecker;
        [SerializeField] private NumberButton[] numberButtons;

        [Serializable]
        private class NumberButton
        {
            public int number;
            public Button button;
        }

        private void Start()
        {
            foreach(var button in numberButtons)
            {
                button.button.OnClickAsObservable()
                    .Subscribe(x =>
                    {
                        var square = squareSelector.selectedSquare;
                        square?.WriteNumber(button.number);

                        answerChecker.SetActiveCheckButton();
                    })
                    .AddTo(this);
            }
        }
    }
}

