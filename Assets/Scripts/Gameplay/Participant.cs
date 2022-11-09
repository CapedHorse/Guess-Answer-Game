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
        public List<QuestionData.AnswerData> answeredDatas;
        
        public int scoreGain;

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
            QuestionData.AnswerData _answerData = null;
            if (GameManager.instance.CurrentQuestion.CheckIfAnswerCorrect(_answer, out _answerData))
            {
                if (answeredDatas.Contains(_answerData)) //already answered this
                {
                    PlayUIManager.instance.OnAnswering(this, "Already Given!");
                    WhenAnswering(AnswerType.Wrong);
                }
                else
                {
                    PlayUIManager.instance.OnAnswering(this, _answer);
                    var pollInDisplay = Mathf.RoundToInt(Mathf.Clamp((float)_answerData.poll / GameManager.instance.gameData.respondenCount, 0, GameManager.instance.gameData.respondenCount) * GameManager.instance.gameData.respondenCountInDisplay);
                    Debug.Log(pollInDisplay + " " + _answerData.poll +" "+ GameManager.instance.gameData.respondenCount+" " + GameManager.instance.gameData.respondenCountInDisplay);
                    GameManager.instance.MoveRespondents(this, pollInDisplay);
                    answeredDatas.Add(_answerData);
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
            answeredDatas.Clear();
        }


    }
}

