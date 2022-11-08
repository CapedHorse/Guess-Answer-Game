using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PikoruaTest
{
    public class Participant : MonoBehaviour
    {
        public enum ControlType { Human, AI }
        public ControlType type;
        public Animator participantAnimator;
        public Color participantColor;
        public Grid respondentArea;
        public GameObject answerPopUp;
        public TextMeshProUGUI answerTextMesh;
        public GameObject correctAnswerSign, wrongAnswerSign;
        

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
                case ControlType.Human:

                    break;
                case ControlType.AI:

                    break;
                default:
                    break;
            }
        }
    }
}

