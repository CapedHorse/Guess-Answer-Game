using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PikoruaTest
{
    /// <summary>
    /// This is AI data, contains data of the AI, to determine how often the AI will answer and how hight the chance to answer correct answer;
    /// </summary>
    [CreateAssetMenu(fileName ="AIData", menuName = "AI Data")]
    public class AIData : ScriptableObject
    {
        public enum AIDifficulty { Easy, Medium, Hard}
        public AIDifficulty difficulty;

        [Tooltip("Interval between answering, percentage of the time, the higher, the longer the interval")][Range(1f,100f)] public float answeringIntervalFromTime;
        [Tooltip("Chance to answer correct answer instead of just 'wrong' ")][Range(1f, 100f)] public float chanceToCorrectAnswer; 
    }
}


