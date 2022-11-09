
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
        public bool CheckIfAnswerCorrect(string _answer, out int _answerId )
        {

            //Simple without regex
            foreach (var answer in answers)
            {
                if (_answer.Equals(answer.answer))
                {
                    _answerId = answer.answerId;
                    return true;
                }
                else
                {
                    foreach (var similar in answer.similarAnswers)
                    {
                        if (similar.Equals(_answer))
                        {
                            _answerId = answer.answerId;
                            return true;
                        }
                       
                    }
                    _answerId = 0;
                    return false;

                }
            }

            //with regex

            _answerId = 0;
            return false;
        }
    }
}

