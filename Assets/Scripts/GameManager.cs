using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameManager : IStartable
{
    private NumberManager numberManager;
    private SquareManager squareManager;

    [Inject]
    public GameManager(NumberManager numberManager, SquareManager squareManager)
    {
        this.numberManager = numberManager;
        this.squareManager = squareManager;
    }

    public void Start()
    {
        squareManager.GenerateSquares();
        numberManager.GenerateNumbers();
    }
}
