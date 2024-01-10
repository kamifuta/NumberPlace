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

        private void Start()
        {
            numberPlaceGenerator.GenerateNumberPlace();
            Debug.Log("êîóÒÇÃê∂ê¨äÆóπ");

            //numberPlaceViewer.ViewNumberPlace();

            //await UniTask.Yield(this.GetCancellationTokenOnDestroy());

            questionGenerator.GenerateQuestion(20);

            questionViewer.ViewQuestion();
        }
    }
}

