using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PikoruaTest
{
    [CreateAssetMenu(fileName ="QuestionDatabase", menuName ="Question Database")]
    public class QuestionDatabase : ScriptableObject
    {
        public List<QuestionData> questionDatas;
    }
}

