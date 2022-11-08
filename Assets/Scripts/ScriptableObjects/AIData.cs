using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PikoruaTest
{
    [CreateAssetMenu(fileName ="AIData", menuName = "AI Data")]
    public class AIData : ScriptableObject
    {
        public enum AIDifficulty { Easy, Medium, Hard}
        public AIDifficulty difficulty;

        [Range(1f,100f)] public float answeringIntervalFromTime; //interval between answering, percentage of the time
        [Range(1f, 100f)] public float chanceToAnswerTheHighestPoll; //chance to answer the highest poll of an answer
    }
}


