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
            Display();
        }

        /// <summary>
        /// Higlighting chosen answer based on their participant color
        /// </summary>
        /// <param name="_participant"></param>
        public void Display(Participant _participant = null)
        {
            if (_participant != null)
            {
                answerImage.color = _participant.participantColor;
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

