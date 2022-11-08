using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PikoruaTest
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        PlayUIManager uIManager;

        [Header("Object Cache")]
        public GameData gameData;
        public Grid respondentGrid;
        public Participant player, enemy;

        [Header("Status")]
        public bool isPlaying;
        public float currentPlayTime;
        public int currentRound;
        public bool gameOver;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;

            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }
        // Start is called before the first frame update
        void Start()
        {            
            uIManager = PlayUIManager.instance;

            


        }

        // Update is called once per frame
        void Update()
        {
            if (isPlaying)
            {
                if (currentPlayTime < gameData.timePerRound)
                {

                }
            }
        }

        void CountDown()
        {
            StartCoroutine(uIManager.CountingDownUI(StartGame));
        }

        void StartGame()
        {

        }

        

        void RoundOver()
        {
            isPlaying = false;
            currentRound++;
            if (currentRound < gameData.roundPerGame)
            {

            }
            else
            {
                GameOver();
            }
        }

        void GameOver()
        {
            gameOver = true;


        }

        void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

