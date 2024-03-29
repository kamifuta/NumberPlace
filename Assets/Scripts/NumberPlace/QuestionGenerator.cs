using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NumberPlace
{
    public class QuestionGenerator : MonoBehaviour
    {
        [SerializeField] private NumberPlaceManager numberPlaceManager;
        [SerializeField] private QuestionManager questionManager;

        public void GenerateQuestion(int emptyAmount)
        {
            for(int x = 0; x < 3; x++)
            {
                for(int y = 0; y < 3; y++)
                {
                    for(int h = 0; h < 3; h++)
                    {
                        for(int v = 0; v < 3; v++)
                        {
                            int number = numberPlaceManager.GetNumber(x, y, h, v);
                            questionManager.SetNumber(x, y, h, v, number);
                        }
                    }
                }
            }

            List<(int x, int y, int h, int v)> list = new List<(int x, int y, int h, int v)>();

            for(int i = 0; i < emptyAmount; i++)
            {
                var x = Random.Range(0, 3);
                var y = Random.Range(0, 3);
                var v = Random.Range(0, 3);
                var h = Random.Range(0, 3);

                if (list.Any(t =>  t== (x, y, h, v)))
                {
                    i--;
                    continue;
                }

                list.Add((x, y, h, v));
                questionManager.SetNumber(x, y, h, v, 0);
            }
        }
    }
}

