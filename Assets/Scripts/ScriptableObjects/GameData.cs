using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace PikoruaTest
{
    [CreateAssetMenu(fileName = "GeneralProperties", menuName ="General Properties")]
    public class GameData : ScriptableObject
    {
        public int respondenCount;
        public int respondenCountInDisplay;
        [Tooltip("Time per round (in seconds)")]
        public float timePerRound;
        public int roundPerGame;

        public Color playerColor, enemyColor;

        public List<Respondent> respondentPrefabs;

        public List<QuestionData> questionDatas;
    }
}

