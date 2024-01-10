using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace NumberPlace
{
    public class QuestionViewer : MonoBehaviour
    {
        [SerializeField] private QuestionManager questionManager;

        [SerializeField] private GameObject[] blockObjectArray;

        private Square[,][,] SquareArray = new Square[3, 3][,];

        private void Start()
        {
            Init();
        }

        private void Init()
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

        public void ViewQuestion()
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    for (int h = 0; h < 3; h++)
                    {
                        for (int v = 0; v < 3; v++)
                        {
                            var number = questionManager.GetNumber(x, y, h, v);

                            if (number == 0)
                            {
                                SquareArray[y, x][v, h].SetActiveNumberText(false);
                                SquareArray[y, x][v, h].SetActiveInputField(true);
                            }
                            else
                            {
                                SquareArray[y, x][v, h].SetActiveNumberText(true);
                                SquareArray[y, x][v, h].SetActiveInputField(false);
                                SquareArray[y, x][v, h].SetNumberText(number);
                            }
                            
                        }
                    }
                }
            }
        }
    }
}

