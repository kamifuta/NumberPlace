using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace NumberPlace
{
    public class NumberPlaceViewer : MonoBehaviour
    {
        [SerializeField] private NumberPlaceManager numberPlaceManager;

        [SerializeField] private GameObject[] blockObjectArray;

        private TMP_Text[,][,] numberText = new TMP_Text[3, 3][,];

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
                    numberText[y, x] = new TMP_Text[3, 3];

                    for (int h = 0; h < 3; h++)
                    {
                        for (int v = 0; v < 3; v++)
                        {
                            numberText[y, x][v, h] = blockObject.transform.GetChild(v * 3 + h).GetComponentInChildren<TMP_Text>();
                        }
                    }
                }
            }
        }

        public void ViewNumberPlace()
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    for (int h = 0; h < 3; h++)
                    {
                        for (int v = 0; v < 3; v++)
                        {
                            numberText[y, x][v, h].text = numberPlaceManager.GetNumber(x, y, h, v).ToString();
                        }
                    }
                }
            }
        }
    }
}
