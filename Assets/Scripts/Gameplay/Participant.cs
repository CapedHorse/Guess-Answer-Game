using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PikoruaTest
{
    public class Participant : MonoBehaviour
    {
        public enum ControlType { Human, AI }
        public ControlType type;
        public Color playerColor;

        void Start()
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

