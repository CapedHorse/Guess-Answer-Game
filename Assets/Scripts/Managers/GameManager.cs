using Lean.Pool;
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
        public List<Respondent> respondents, playerRespondents, enemyRespondents;

        [Header("Status")]
        public bool isPlaying;
        public float currentPlayTime;
        public int currentRound = 0;
        public int rewardGain;
        public bool gameOver;

        public List<QuestionData> questions;
        public List<QuestionData.AnswerData> answered;
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

            InitRespondents();
            InitQuestions();            
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
                    PlayUIManager.instance.UpdateTimerUI(currentPlayTime);
                }
                else
                {
                    RoundOver();
                }
            }
        }

        void InitRespondents()
        {
            respondentGrid.Init();
            respondents = new List<Respondent>();
            playerRespondents = new List<Respondent>();
            enemyRespondents = new List<Respondent>();

            for (int i = 0; i < gameData.respondenCountInDisplay; i++)
            {
                var respondent = LeanPool.Spawn(gameData.respondentPrefabs[Random.Range(0, gameData.respondentPrefabs.Count - 1)], respondentGrid.gridParent);
                respondent.SetStop();
                respondent.transform.position = respondentGrid.gridPoints[i].position;  
                respondent.initPoint = respondentGrid.gridPoints[i];
                respondentGrid.gridPoints[i].occupied = true;
                respondents.Add(respondent);
            }
        }

        void InitQuestions()
        {
            questions = new List<QuestionData>();
            answered = new List<QuestionData.AnswerData>();
            for (int i = 0; i < gameData.roundPerGame; i++)
            {
                var availableQuestions = gameData.questionDatas.FindAll(x => !questions.Contains(x));
                questions.Add(availableQuestions[Random.Range(0, availableQuestions.Count - 1)]);
            }
        }

        void CountDown()
        {
            StartCoroutine(uIManager.CountingDownUI(StartGame));
        }

        void StartGame()
        {
            currentPlayTime = 0;
            
            isPlaying = true;
        }        

        /// <summary>
        /// while a round is over, answer ui
        /// </summary>
        void RoundOver()
        {
            isPlaying = false;
            PlayUIManager.instance.NotifyRoundOver();        

        }


        public void MoveRespondents(Participant _participant, int _amount)
        {
            _amount = Mathf.Clamp(_amount, 0, respondents.Count);
            for (int i = 0; i < _amount; i++)
            {
                var movedRespondent = respondents[Random.Range(0, respondents.Count - 1)];
                var availableGridPoint = _participant.respondentArea.gridPoints.Find(x => !x.occupied);
                movedRespondent.SetGoToPoll(availableGridPoint.position);
                availableGridPoint.occupied = true;
                switch (_participant.type)
                {
                    case Participant.ControlType.Player:
                        playerRespondents.Add(movedRespondent);
                        break;
                    case Participant.ControlType.Enemy:
                        enemyRespondents.Add(movedRespondent);
                        break;
                    default:
                        break;
                }

                respondents.Remove(movedRespondent);
            }
        }

        public void ResetParticipantsRespondent()
        {
            foreach (var item in playerRespondents)
            {
                item.Reset();
                respondents.Add(item);
            }
            playerRespondents.Clear();

            foreach (var item in enemyRespondents)
            {
                item.Reset();
                respondents.Add(item);
            }
            enemyRespondents.Clear();

            foreach (var item in player.respondentArea.gridPoints)
            {
                item.occupied = false;
            }

            foreach (var item in enemy.respondentArea.gridPoints)
            {
                item.occupied = false;
            }

            answered.Clear();

            player.Reset();
            enemy.Reset();
        }

        public void DoubledReward()
        {
            player.doubled = true;
            PlayUIManager.instance.DoublingRewardUI();
            
        }

        void GameOver()
        {
            gameOver = true;

            PlayUIManager.instance.ShowFinalResultUI();
        }

        public void ProceedNextRound()
        {
            currentRound++;
            if (currentRound < gameData.roundPerGame)
            {
                ResetParticipantsRespondent();
                //StartCountdown again but show score multiplier Ui
                CountDown();
            }
            else
            {
                GameOver();
            }
        }

        public void RestartGame()
        {
            SceneManager.LoadScene("PlayScene");
        }

        public void BackToMenu()
        {
            SceneManager.LoadScene("MenuScene");
        }
    }
}

