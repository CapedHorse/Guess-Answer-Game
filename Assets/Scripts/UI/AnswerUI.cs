using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PikoruaTest
{
    public class AnswerUI : MonoBehaviour
    {
        public Image answerImage;
        public TextMeshProUGUI answerText;
        public TextMeshProUGUI answerPollText;

        public void Init(QuestionData.AnswerData _answerData)
        {
            answerText.text = _answerData.answer;
            answerPollText.text = _answerData.poll.ToString();

           
        }

        public void Display(Participant _participant = null)
        {
            if (_participant != null)
            {
                answerImage.color = _participant.playerColor;
                answerText.color = Color.white;
                answerPollText.color = Color.white;
            }
            else
            {
                answerImage.color = Color.white;
                answerText.color = Color.black;
                answerPollText.color = Color.black;
            }
        }
    }
}

