using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PikoruaTest
{
    [CreateAssetMenu(fileName = "GeneralProperties", menuName ="General Properties")]
    public class GeneralProperties : ScriptableObject
    {
        public int respondenCount;
        public int respondenCountInDisplay;
        public float timePerRound;

        public List<Respondent> respondentPrefabs;

    }
}

