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

        [SerializeField] private GameObject retirePanel;
        [SerializeField] private Button retireButton;
        [SerializeField] private Button retireCloseButton;
        [SerializeField] private Button reallyRetireButton;

        private void Start()
        {
            retryButton.onClick.AddListener(() => LoadNextScene("MainScene"));
            backTitleButton.onClick.AddListener(() => LoadNextScene("TitleScene"));
            closeButton.onClick.AddListener(() => CloseResultPanel());
            openButton.onClick.AddListener(() => OpenResultPanel());

            retireButton.onClick.AddListener(() => OpenRetirePanel());
            retireCloseButton.onClick.AddListener(() => CloseRetirePanel());
            reallyRetireButton.onClick.AddListener(() => Retire());

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
                case ResultType.Retire:
                    resultText.text = "Retired...";
                    break;
            }

            resultPanel.SetActive(true);
        }

        private void CloseResultPanel()
        {
            resultPanel.SetActive(false);
            openButton.gameObject.SetActive(true);
        }

        private void OpenResultPanel()
        {
            resultPanel.SetActive(true);
        }

        private void CloseRetirePanel()
        {
            retirePanel.SetActive(false);
        }

        private void OpenRetirePanel()
        {
            retirePanel.SetActive(true);
        }

        private void Retire()
        {
            retireButton.interactable = false;

            answerChecker.Retire();
            CloseRetirePanel();
            ViewResult(ResultType.Retire);
        }

        private void LoadNextScene(string sceneName)
        {
            retryButton.interactable = false;
            backTitleButton.interactable = false;

            SceneManager.LoadScene(sceneName);
        }
    }
}

