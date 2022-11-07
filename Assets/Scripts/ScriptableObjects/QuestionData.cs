
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

        public bool CheckIfAnswerCorrect(string _answer, out int _answerId)
        {
            _answerId = 0;
            return true;
        }
    }
}

