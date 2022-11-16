using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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
        
        public int scoreGain, totalScore;
        public bool doubled;
        public float timePassed;

        //determine interval between time passed
        public float aiAnsweringTime => Mathf.Round(GameManager.instance.gameData.CurrentAI.answeringIntervalFromTime/100 *  GameManager.instance.gameData.timePerRound);

        void Start()
        {
            respondentArea.Init();
        }

        void Update()
        {
            switch (type)
            {
                case ControlType.Player:

                    break;
                case ControlType.Enemy:
                    if (!GameManager.instance.isPlaying)
                        return;
                    if (timePassed < aiAnsweringTime)
                    {
                        timePassed+= Time.deltaTime;
                    }
                    else
                    {
                        timePassed = 0;
                        AIAnswer();
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Answering wether from input field or from AI Answering mechanism for enemy
        /// </summary>
        /// <param name="_answer"></param>
        public void Answer(string _answer)
        {
            if (!GameManager.instance.isPlaying)
                return;

            QuestionData.AnswerData _answerData = null;
            if (GameManager.instance.CurrentQuestion.CheckIfAnswerCorrect(_answer, out _answerData))
            {
                if (answeredDatas.Contains(_answerData)) //already answered this
                {
                    PlayUIManager.instance.ParticipantAnswered(this, "Already Given!");
                    StartCoroutine(WhenAnswering(AnswerType.Wrong));
                }
                else
                {
                    PlayUIManager.instance.ParticipantAnswered(this, _answer);
                    PlayUIManager.instance.UpdateParticipantUI(type, scoreGain+_answerData.poll);
                    var pollInDisplay = Mathf.RoundToInt(Mathf.Clamp((float)_answerData.poll / GameManager.instance.gameData.respondenCount, 0, GameManager.instance.gameData.respondenCount) * GameManager.instance.gameData.respondenCountInDisplay);
                    Debug.Log(pollInDisplay + " " + _answerData.poll +" "+ GameManager.instance.gameData.respondenCount+" " + GameManager.instance.gameData.respondenCountInDisplay);
                    GameManager.instance.MoveRespondents(this, pollInDisplay);
                    GameManager.instance.answered.Add(_answerData);
                    
                    scoreGain += _answerData.poll + Mathf.RoundToInt(_answerData.poll* GameManager.instance.gameData.scoreMultiplier * GameManager.instance.currentRound);
                    StartCoroutine(WhenAnswering(AnswerType.Correct));
                    answeredDatas.Add(_answerData);
                }
            }
            else
            {
                PlayUIManager.instance.ParticipantAnswered(this, "WrongAnswer");
                StartCoroutine(WhenAnswering(AnswerType.Wrong));
            }
        }

        /// <summary>
        /// AI Answering
        /// Use chance to determine wether to answer correct answer or just "wrong"
        /// Will answer from the lowest poll
        /// </summary>
        public void AIAnswer()
        {
            Debug.Log("AI Answering");
            if (Random.Range(0,100f) >= GameManager.instance.gameData.CurrentAI.chanceToCorrectAnswer)
            {
                var availableAnswer = GameManager.instance.CurrentQuestion.answers.FindAll(x => !GameManager.instance.answered.Contains(x));

                for (int i = GameManager.instance.CurrentQuestion.answers.Count -1; i > 0; i--)
                {
                    var answer = GameManager.instance.CurrentQuestion.answers[i];
                    if (GameManager.instance.answered.Contains(answer))
                        continue;
                    Answer(answer.answer);
                    break;
                }


            }
        }

        public IEnumerator WhenAnswering(AnswerType type)
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
            yield return new WaitForSeconds(1f);
            answerObj.SetActive(false);            
        }

        public void GainTotalScore()
        {
            totalScore += scoreGain * (doubled ? 2 : 1);
        }

        public void Reset()
        {
            answeredDatas.Clear();
            timePassed = 0;
            totalScore += scoreGain;
            scoreGain = 0;
            doubled = false;
        }


    }
}

