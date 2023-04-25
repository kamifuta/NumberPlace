using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareGenerator : MonoBehaviour
{
    [SerializeField] private GameObject squarePrefab;
    [SerializeField] private Transform squareParent;

    public GameObject[,] GenerateSquares()
    {
        GameObject[,] result = new GameObject[9,9];
        for (int i = -4; i <= 4; i++)
        {
            for(int j = 0; j < 9; j++)
            {
                var square = Instantiate(squarePrefab);
                square.transform.SetParent(squareParent);
                square.transform.localPosition = new Vector3(i * 90, j * 90, 0);

                result[i + 4, j] = square;
            }
        }
        return result;
    }
}
