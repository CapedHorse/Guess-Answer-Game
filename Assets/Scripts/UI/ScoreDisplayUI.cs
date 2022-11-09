using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PikoruaTest
{
    public class ScoreDisplayUI : MonoBehaviour
    {
        public TextMeshProUGUI playerScoreText, enemyScoreText;

        public void Init( int _playerScore, int _enemyScore)
        {
            playerScoreText.text = _playerScore.ToString();
            enemyScoreText.text = _enemyScore.ToString();
        }
    }
}

