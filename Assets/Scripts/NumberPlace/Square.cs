using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace NumberPlace
{
    public class Square : MonoBehaviour
    {
        [SerializeField] private TMP_Text numberText;
        [SerializeField] private TMP_InputField inputField;

        public void SetActiveInputField(bool value)
        {
            inputField.gameObject.SetActive(value);
        }

        public void SetActiveNumberText(bool value)
        {
            numberText.gameObject.SetActive(value);
        }

        public void SetNumberText(int number)
        {
            numberText.text = number.ToString();
        }
    }
}

