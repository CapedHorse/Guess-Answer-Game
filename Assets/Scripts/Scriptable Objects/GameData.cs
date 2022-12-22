using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace PikoruaTest
{
    [CreateAssetMenu(fileName = "GeneralProperties", menuName ="General Properties")]
    public class GameData : ScriptableObject
    {
        [Header("GamePlay")]
        public int respondenCount;
        public int respondenCountInDisplay;
        [Tooltip("Time per round (in seconds)")]
        public float timePerRound;
        [Range(2,5)]public int scoreMultiplier;
        public int roundPerGame;
        public AIData.AIDifficulty AIDifficulty;
        public AIData CurrentAI => aiDatas.Find(x => x.difficulty == AIDifficulty);

        //public Color playerColor, enemyColor;

        public List<Respondent> respondentPrefabs;

        public List<QuestionData> questionDatas;

        public List<AIData> aiDatas;

    }
}

