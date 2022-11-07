using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PikoruaTest
{
    [CustomEditor(typeof(QuestionData))]
    public class QuestionDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var questionDataSO = (QuestionData)target;
            
            var generalSettings = EditorGUIUtility.Load("Assets/Resources/GeneralSettings.asset") as GameData;
            var maxRespondent = generalSettings.respondenCount;

            //Checking total popularity gained, also set each answer's id
            int totalPopularity = 0;
            foreach (var item in questionDataSO.answers)
            {
                totalPopularity += item.poll;
                item.answerId = questionDataSO.answers.IndexOf(item);
            }

            //if not exceeding maximum yet
            if (totalPopularity <= maxRespondent)
            {
                questionDataSO.respondentLeftForPopularity = maxRespondent - totalPopularity;
            }
            else //if do, automatically set last answer's popularity to remaining popularity
            {
                int totalPopularityBeforeLast = 0;
                for (int i = 0; i < questionDataSO.answers.Count -1; i++)
                {
                    totalPopularityBeforeLast += questionDataSO.answers[i].poll;
                }

                questionDataSO.answers[questionDataSO.answers.Count - 1].poll = Mathf.Clamp(maxRespondent - totalPopularityBeforeLast, 0, maxRespondent);
                questionDataSO.respondentLeftForPopularity = 0;

                Debug.LogWarning("Popularity count exceeding maximum respondent.");
            }
        }
    }
}

