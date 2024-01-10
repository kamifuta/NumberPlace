using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NumberPlace
{
    public class QuestionManager : MonoBehaviour
    {
        private readonly int[,][,] question = new int[3, 3][,];

        private void Awake()
        {
            InitNumberPlace();
        }

        private void InitNumberPlace()
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    question[y, x] = new int[3, 3];

                    for (int v = 0; v < 3; v++)
                    {
                        for (int h = 0; h < 3; h++)
                        {
                            question[y, x][v, h] = 0;
                        }
                    }
                }
            }
        }

        public void SetNumber(int x, int y, int h, int v, int n)
        {
            question[y, x][v, h] = n;
        }

        public int GetNumber(int x, int y, int h, int v)
        {
            return question[y, x][v, h];
        }
    }
}

