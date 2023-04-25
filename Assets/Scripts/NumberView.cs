using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumberView : MonoBehaviour
{
    public void ViewNumber(TMP_Text text, int number)
    {
        text.text = number.ToString();
    }
}
