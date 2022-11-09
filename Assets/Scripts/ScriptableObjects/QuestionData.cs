
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PikoruaTest
{
    [CreateAssetMenu(fileName = "QuestionData", menuName = "Question Data")]
    public class QuestionData : ScriptableObject
    {
        public int questionId;
        public string question;
        public List<AnswerData> answers;

        [ReadOnly] public int respondentLeftForPopularity;

        [System.Serializable]
        public class AnswerData
        {
            public string answer;
            public int answerId;
            public int poll;
            public List<string> similarAnswers;
        }

        /// <summary>
        /// Method to check whether the answer given by player is exist or not 
        /// </summary>
        /// <param name="_answer"></param>
        /// <param name="_answerId"></param>
        /// <returns></returns>
        public bool CheckIfAnswerCorrect(string _answer, out AnswerData _answerData )
        {

            //Simple without regex
            //ADA KESALAHAN, HARUSNYA NGECEK TIAP LOOP DULU
            foreach (var answer in answers)
            {
                if (_answer.Equals(answer.answer))
                {
                    Debug.Log("Answer is " + _answer + " and is " + answer.answer+" So Correct ");
                    _answerData = answer;
                    return true;
                }
                else
                {
                    foreach (var similar in answer.similarAnswers)
                    {
                        if (similar.Equals(_answer))
                        {
                            Debug.Log("Answer is " + _answer + " and is " + answer.answer + " So Correct ");
                            _answerData = answer;
                            return true;
                        }
                       
                    }

                    continue;
                }

                
            }

            Debug.Log("Wrong");
            _answerData = null;
            return false;

            //with regex
        }
    }
}

