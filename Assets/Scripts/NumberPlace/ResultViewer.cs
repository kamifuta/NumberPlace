using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.SceneManagement;

namespace NumberPlace
{
    public class ResultViewer : MonoBehaviour
    {
        [SerializeField] private AnswerChecker answerChecker;

        [SerializeField] private GameObject resultPanel;
        [SerializeField] private TMP_Text resultText;

        [SerializeField] private Button retryButton;
        [SerializeField] private Button backTitleButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button openButton;

        private void Start()
        {
            retryButton.onClick.AddListener(() => LoadNextScene("MainScene"));
            backTitleButton.onClick.AddListener(() => LoadNextScene("TitleScene"));
            closeButton.onClick.AddListener(() => ClosePanel());
            openButton.onClick.AddListener(() => OpenPanel());

            answerChecker.resultObservable
                .Subscribe(result =>
                {
                    ViewResult(result);
                })
                .AddTo(this);
        }

        private void ViewResult(ResultType result)
        {
            switch (result)
            {
                case ResultType.Success:
                    resultText.text = "SUCCESS!";
                    break;
                case ResultType.Failure:
                    resultText.text = "Failed...";
                    break;
            }

            resultPanel.SetActive(true);
        }

        private void ClosePanel()
        {
            resultPanel.SetActive(false);
            openButton.gameObject.SetActive(true);
        }

        private void OpenPanel()
        {
            resultPanel.SetActive(true);
        }

        private void LoadNextScene(string sceneName)
        {
            retryButton.interactable = false;
            backTitleButton.interactable = false;

            SceneManager.LoadScene(sceneName);
        }
    }
}

