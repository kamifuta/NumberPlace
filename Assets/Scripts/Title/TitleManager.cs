using NumberPlace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Title
{
    public class TitleManager : MonoBehaviour
    {
        [SerializeField] private Button easyButton;
        [SerializeField] private Button normalButton;
        [SerializeField] private Button hardButton;

        private void Start()
        {
            easyButton.onClick.AddListener(() => LoadMainScene(DifficultType.Easy));
            normalButton.onClick.AddListener(() => LoadMainScene(DifficultType.Normal));
            hardButton.onClick.AddListener(() => LoadMainScene(DifficultType.Hard));
        }

        private void LoadMainScene(DifficultType difficultType)
        {
            easyButton.interactable = false;
            normalButton.interactable = false;
            hardButton.interactable = false;

            GameSetting.difficultType = difficultType;
            SceneManager.LoadScene("MainScene");
        }
    }
}

