using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PikoruaTest
{
    public class Participant : MonoBehaviour
    {
        public enum ControlType { Player, Enemy }
        public enum AnswerType { Correct, Wrong }
        public ControlType type;
        public Animator participantAnim;
        public Color participantColor;
        public Grid respondentArea;
        public GameObject correctAnswerSign, wrongAnswerSign;
        public List<int> answeredIds;

        void Start()
        {
            respondentArea.Init();
        }

        //should initiate the color
        public void Init()
        {

        }

        void Update()
        {
            switch (type)
            {
                case ControlType.Player:

                    break;
                case ControlType.Enemy:

                    break;
                default:
                    break;
            }
        }

        public void Answer(string _answer)
        {
            int answerId = 0;
            if (GameManager.instance.CurrentQuestion.CheckIfAnswerCorrect(_answer, out answerId))
            {
                if (answeredIds.Contains(answerId)) //already answered this
                {
                    PlayUIManager.instance.OnAnswering(this, "Already Given!");
                    WhenAnswering(AnswerType.Wrong);
                }
                else
                {
                    PlayUIManager.instance.OnAnswering(this, _answer);
                    answeredIds.Add(answerId);
                    WhenAnswering(AnswerType.Correct);
                }
            }
            else
            {
                PlayUIManager.instance.OnAnswering(this, "WrongAnswer");
                WhenAnswering(AnswerType.Wrong);
            }
        }

        public void WhenAnswering(AnswerType type)
        {
            participantAnim.SetTrigger("Answering");
            GameObject answerObj = null;
            switch (type)
            {
                case AnswerType.Correct:
                    answerObj = correctAnswerSign;
                    break;
                case AnswerType.Wrong:
                    answerObj = wrongAnswerSign;
                    break;
                default:
                    break;
            }

            answerObj.SetActive(true);
            DOVirtual.DelayedCall(0.25f, () => answerObj.SetActive(false));
        }

        public void Reset()
        {
            answeredIds.Clear();
        }


    }
}

