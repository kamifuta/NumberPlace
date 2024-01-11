using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NumberPlace
{
    public class SquareManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] blockObjectArray;
        private readonly Square[,][,] SquareArray = new Square[3, 3][,];

        public void Init()
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    var blockObject = blockObjectArray[y * 3 + x];
                    SquareArray[y, x] = new Square[3, 3];

                    for (int h = 0; h < 3; h++)
                    {
                        for (int v = 0; v < 3; v++)
                        {
                            SquareArray[y, x][v, h] = blockObject.transform.GetChild(v * 3 + h).GetComponent<Square>();
                        }
                    }
                }
            }
        }

        public Square GetSquare(int x, int y, int h, int v)
        {
            return SquareArray[y, x][v, h];
        }
    }
}
