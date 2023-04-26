using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;
using VContainer.Unity;
using VContainer;
using System;

public class NumberPresenter : ControllerBase, IInitializable
{
    private NumberView numberView;
    private NumberManager numberManager;
    private SquareManager squareManager;

    [Inject]
    public NumberPresenter(NumberView numberView, NumberManager numberManager, SquareManager squareManager)
    {
        this.numberView = numberView;
        this.numberManager = numberManager;
        this.squareManager = squareManager;
    }

    public void Initialize()
    {
        ObserveGeneratedNumbers();
    }

    private void ObserveGeneratedNumbers()
    {
        numberManager.NumberObservable
            .Subscribe(numbers =>
            {
                for(int i = 0; i < 3; i++)
                {
                    for(int j = 0; j < 3; j++)
                    {
                        for(int k = 0; k < 3; k++)
                        {
                            for(int l = 0; l < 3; l++)
                            {
                                int squareX = i * 3 + k;
                                int squareY = 8-(j * 3 + l);

                                try
                                {
                                    var square = squareManager.squares[squareX, squareY];
                                    var num = numbers[i, j][k, l];

                                    var text = square.GetComponentInChildren<TMP_Text>();
                                    numberView.ViewNumber(text, num);
                                }
                                catch (NullReferenceException)
                                {
                                    Debug.Log($"i:{i}, j:{j}, k:{k}, l:{l}");
                                    return;
                                }
                                
                            }
                        }
                    }
                }
            })
            .AddTo(this);
    }
}
