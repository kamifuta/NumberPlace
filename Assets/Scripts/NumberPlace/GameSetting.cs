using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NumberPlace
{
    public static class GameSetting
    {
        public static DifficultType difficultType;

        public static int GetEmptyAmount()
        {
            switch (difficultType)
            {
                case DifficultType.Easy:
                    return Random.Range(20, 26);
                case DifficultType.Normal:
                    return Random.Range(30, 36);
                case DifficultType.Hard:
                    return Random.Range(45, 51);
            }

            return 20;
        }
    }
}

