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
        [SerializeField] private SpriteRenderer spriteRenderer;

        private bool IsWritable = true;

        public string NumberText => numberText.text;

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
            if (number == 0)
            {
                numberText.text = "";
                IsWritable = true;
                return;
            }

            numberText.text = number.ToString();
            numberText.fontStyle = FontStyles.Bold;
            IsWritable = false;
        }

        public void SetSelected(bool value)
        {
            if (value)
            {
                spriteRenderer.color = new Color(0, 1, 0, 1);
            }
            else
            {
                spriteRenderer.color = new Color(1, 1, 1, 1);
            }
        }

        public void SetEmphasis(bool value)
        {
            if (value)
            {
                spriteRenderer.color = new Color(0, 1, 0, 0.3f);
            }
            else
            {
                spriteRenderer.color = new Color(1, 1, 1, 1);
            }
        }

        public void WriteNumber(int number)
        {
            if (!IsWritable)
            {
                return;
            }

            numberText.text = number.ToString();
        }

        public void SetTextColor(Color color)
        {
            numberText.color = color;
        }
    }
}

