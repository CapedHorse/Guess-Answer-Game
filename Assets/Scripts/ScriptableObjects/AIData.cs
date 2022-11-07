using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PikoruaTest
{
    public class AIData : ScriptableObject
    {
        public enum AIDifficulty { Easy, Medium, Hard}
        public AIDifficulty difficulty;

        public float answeringInterval;
        public float chanceToAnswerTheHighestPoll;
    }
}


