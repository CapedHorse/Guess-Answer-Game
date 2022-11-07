using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PikoruaTest
{
    [CustomEditor(typeof(QuestionDatabase))]
    public class QuestionDatabaseEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var questionDatabase = (QuestionDatabase)target;
            questionDatabase.questionDatas.Clear();

            Debug.Log("Questions count" + Resources.LoadAll("Questions", typeof(QuestionData)).Length); 
            foreach (var item in Resources.LoadAll("Questions", typeof(QuestionData)))
            {
                Debug.Log("Question " + item.name);
                questionDatabase.questionDatas.Add((QuestionData)item);
            }
        }
    }
}

