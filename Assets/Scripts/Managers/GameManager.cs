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

        public List<QuestionData> questions;
        public QuestionData CurrentQuestion => questions[currentRound];
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

            CountDown();


        }

        // Update is called once per frame
        void Update()
        {
            if (isPlaying)
            {
                if (currentPlayTime < gameData.timePerRound)
                {
                    currentPlayTime += Time.deltaTime;
                    PlayUIManager.instance.UpdateTimerUI();
                }
                else
                {
                    RoundOver();
                }
            }
        }

        void CountDown()
        {
            StartCoroutine(uIManager.CountingDownUI(StartGame));
        }

        void StartGame()
        {
            currentPlayTime = 0;
            currentRound = 0;
            isPlaying = true;
        }

        

        void RoundOver()
        {
            isPlaying = false;
            currentRound++;
            if (currentRound < gameData.roundPerGame)
            {
                //Show result UI then Continue
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

