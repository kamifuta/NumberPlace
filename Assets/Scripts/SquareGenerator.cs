using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareGenerator : MonoBehaviour
{
    [SerializeField] private GameObject squarePrefab;
    [SerializeField] private Transform squareParent;

    private void Start()
    {
        GenerateSquare();
    }

    public void GenerateSquare()
    {
        for (int i = -4; i <= 4; i++)
        {
            for(int j = 0; j < 9; j++)
            {
                var square = Instantiate(squarePrefab);
                square.transform.SetParent(squareParent);
                square.transform.localPosition = new Vector3(i * 90, j * 90, 0);
            }
        }
    }
}
