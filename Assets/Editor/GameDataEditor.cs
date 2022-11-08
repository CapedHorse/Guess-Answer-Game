using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PikoruaTest
{
    [CustomEditor(typeof(GameData))]
    public class GameDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var gameData = (GameData)target;
            gameData.questionDatas.Clear();
            gameData.aiDatas.Clear();
            //gameData.respondentPrefabs.Clear();

            Debug.Log("Questions count" + Resources.LoadAll("Questions", typeof(QuestionData)).Length); 
            foreach (var item in Resources.LoadAll("Questions", typeof(QuestionData)))
            {
                Debug.Log("Question " + item.name);
                gameData.questionDatas.Add((QuestionData)item);
            }

            Debug.Log("AI Data count" + Resources.LoadAll("AIData", typeof(AIData)).Length);
            foreach (var item in Resources.LoadAll("AIData", typeof(AIData)))
            {
                Debug.Log("AI Data " + item.name);
                gameData.aiDatas.Add((AIData)item);
            }

            //Debug.Log("Respondent count" + AssetDatabase.LoadAllAssetsAtPath.LoadPrefabContents("Assets/Prefabs/Respondents").Length);
            //foreach (var item in PrefabUtility.LoadPrefabContents("Assets/Prefabs/Respondents"))
            //{
            //    gameData.respondentPrefabs.Add((Respondent)item);
            //}
        }
    }
}


