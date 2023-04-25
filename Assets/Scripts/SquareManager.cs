using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class SquareManager
{
    private SquareGenerator squareGenerator;

    public GameObject[,] squares { get; private set; } = new GameObject[3, 3];

    [Inject]
    public SquareManager(SquareGenerator squareGenerator)
    {
        this.squareGenerator = squareGenerator;
    }

    public void GenerateSquares()
    {
        squares = squareGenerator.GenerateSquares();
    }
}
