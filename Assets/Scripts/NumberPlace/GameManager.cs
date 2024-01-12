using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NumberPlace
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private NumberPlaceGenerator numberPlaceGenerator;
        [SerializeField] private NumberPlaceViewer numberPlaceViewer;

        [SerializeField] private QuestionGenerator questionGenerator;
        [SerializeField] private QuestionViewer questionViewer;

        [SerializeField] private NumberPlaceResolver numberPlaceResolver;

        [SerializeField] private SquareManager squareManager;
        [SerializeField] private SquareSelector squareSelector;

        private async void Start()
        {
            squareManager.Init();

            numberPlaceGenerator.GenerateNumberPlace();
            Debug.Log("êîóÒÇÃê∂ê¨äÆóπ");

            int emptyAmount = GameSetting.GetEmptyAmount();
            Debug.Log(emptyAmount);

            while (true)
            {
                questionGenerator.GenerateQuestion(emptyAmount);
                questionViewer.ViewQuestion();

                squareSelector.SetSelectedAction();

                var success=await numberPlaceResolver.Resolve();

                if (success)
                    break;
            }
        }
    }
}

