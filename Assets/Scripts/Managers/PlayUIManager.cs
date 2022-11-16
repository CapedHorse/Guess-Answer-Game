using DG.Tweening;
using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PikoruaTest
{
    public class PlayUIManager : MonoBehaviour
    {
        public static PlayUIManager instance;

        public Canvas playCanvas;

        //Script References
        GameManager gameManager;

        [Header("Gameplay")]
        public GameObject playUI;
        public TextMeshProUGUI questionText;
        public Image playerScoreFill, enemyScoreFill;
        //public Image playerScoreBg, enemyScoreBg;
        public TextMeshProUGUI playerScoreText, enemyScoreText;
        public GameObject answerInputPanel, answerShowPanel;
        public GameObject playerAnswerPopUp, enemyAnswerPopUp;
        public TextMeshProUGUI playerAnswerPopUpText, enemyAnswerPopUpText;
        public TMP_InputField answerInputField;
        public Button answerButton, continueAfterAnswerButton;
        public Image timerIndicator;
        public TextMeshProUGUI answersTitleText;
        public RectTransform answersParent;
        public AnswerUI answerUIPrefab;
        public Dictionary<int, AnswerUI> answers;

        [Header("Notification")]
        public GameObject roundOvernotif;
        public TextMeshProUGUI roundWinnerText;

        [Header("Result")]
        public GameObject resultUI;
        public GameObject rewardPanel;
        public ScoreDisplayUI scoreDisplayUIPrefab;
        public RectTransform scoreTableParent;
        public List<ScoreDisplayUI> scoreDisplays;
        public TextMeshProUGUI playerTotalPointText, enemyTotalPointText, playerRewardText, rewardGainedText;
        public Button continueButton, doubleRewardButton, noThanksButton;

        [Header("Countdown")]
        public GameObject countDownUI;
        public CanvasGroup countDownCG;
        public GameObject scoreMultipliedNotif;
        public TextMeshProUGUI currentRoundText, countDownText;

        [Header("Game Over")]
        public GameObject gameOverUI;
        public TextMeshProUGUI finalWinnerText, finalScoreText;
        public Button restartButton, quitButton;

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

            gameManager = GameManager.instance;

            answers = new Dictionary<int, AnswerUI>();
        }
        
        void Start()
        {
            

            Init();
        }

        

        /// <summary>
        /// Initiate the UI
        /// </summary>
        void Init()
        {
            //initiate buttons
            answerButton.onClick.AddListener(() =>
            {
                gameManager.player.Answer(answerInputField.text);
                answerInputField.text = "";
            });
            continueAfterAnswerButton.onClick.AddListener(() =>
            {
                //close answers ui and show result UI and init them
                CloseAnswersUI();
            });
            continueButton.onClick.AddListener(() =>
            {
                //close result button and proceed to next round
                CloseResultUI();
            });
            noThanksButton.onClick.AddListener(() =>
            {
                //close the reward gain UI
                UpdateReward();
                
            });
            doubleRewardButton.onClick.AddListener(() =>
            {
                AdsManager.instance.ShowRewarded();
            });
            restartButton.onClick.AddListener(() =>
            {
                gameManager.RestartGame();
            });
            quitButton.onClick.AddListener(() =>
            {
                gameManager.BackToMenu();
            });

            UpdateParticipantUI(Participant.ControlType.Player, 0);
            UpdateParticipantUI(Participant.ControlType.Enemy, 0);

        }

        /// <summary>
        /// Coroutine of Counting Down
        /// </summary>
        /// <param name="_afterCountingDown"></param>
        /// <returns></returns>
        public IEnumerator CountingDownUI(UnityAction _afterCountingDown)
        {
            playUI.SetActive(false);
            currentRoundText.text = "Round " +(gameManager.currentRound+1).ToString();
            scoreMultipliedNotif.SetActive(gameManager.currentRound > 0);
            countDownCG.alpha = 0;
            countDownUI.SetActive(true);
            countDownCG.DOFade(1, 0.25f);                
            countDownText.text = "3";
            yield return new WaitForSeconds(1);
            countDownText.text = "2";
            yield return new WaitForSeconds(1);
            countDownText.text = "1";
            yield return new WaitForSeconds(1);
            countDownText.text = "GO!";
            yield return new WaitForSeconds(1);
            
            playUI.SetActive(true);
            InitQuestion();
            _afterCountingDown?.Invoke();
            countDownCG.DOFade(0, 0.25f).onComplete = () =>
            {
                countDownUI.SetActive(false);
                countDownCG.alpha = 1;
            };
        }

        public void InitQuestion()
        {
            var _questionData = gameManager.CurrentQuestion;
            questionText.text = _questionData.question;

            foreach (var item in answers)
            {
                LeanPool.Despawn(item.Value.gameObject);
            }

            answers.Clear();

            answerInputField.text = "";
            answersTitleText.text = "Top " + _questionData.answers.Count + " Answers";
            foreach (var item in _questionData.answers)
            {
                var answer = LeanPool.Spawn(answerUIPrefab, answersParent);
                answer.Init(item);
                answers.Add(item.answerId, answer);
            }
        }

        public void ParticipantAnswered(Participant _participant, string _answer)//no need to use unity event
        {
            switch (_participant.type)
            {
                case Participant.ControlType.Player:
                    playerAnswerPopUpText.text = _answer;
                    PopUpAnswer(playerAnswerPopUp.transform);
                    break;
                case Participant.ControlType.Enemy:
                    if (_answer != "wrong") //for enemy AI, only pop up if answered "wrong"
                    {
                        enemyAnswerPopUpText.text = _answer;
                        PopUpAnswer(enemyAnswerPopUp.transform);
                    }
                    
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Updating timer UI, modifying timer image's fill amount every update until playtime is over.
        /// </summary>
        public void UpdateTimerUI(float _timeValue)
        {
            var percentage = Mathf.Clamp((gameManager.gameData.timePerRound- _timeValue) / gameManager.gameData.timePerRound * 1, 0, 1);
            timerIndicator.fillAmount = Mathf.Clamp(percentage,0, 1);
        }

        /// <summary>
        /// Updating participant UI like the respondent bar and amount on text
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        public void UpdateParticipantUI(Participant.ControlType type, int value)
        {
            switch (type)
            {
                case Participant.ControlType.Player:
                    playerScoreFill.fillAmount = Mathf.Clamp((float) value / gameManager.gameData.respondenCount * 1, 0, 1);
                    playerScoreText.text = value.ToString();
                    break;
                case Participant.ControlType.Enemy:
                    enemyScoreFill.fillAmount = Mathf.Clamp((float) value / gameManager.gameData.respondenCount * 1, 0, 1);
                    enemyScoreText.text = value.ToString();
                    break;
                default:
                    break;
            }
        }
        
        /// <summary>
        /// Function to tween popped up answer based from any participant
        /// </summary>
        /// <param name="_popedUpAnswer"></param>
        void PopUpAnswer(Transform _popedUpAnswer)
        {
            DOTween.Kill(_popedUpAnswer);
            
            _popedUpAnswer.DOScale(0, 0).onComplete = () => _popedUpAnswer.gameObject.SetActive(true);
            _popedUpAnswer.DOScale(Vector3.one * 1.5f, 0.25f).onComplete = () =>
            _popedUpAnswer.DOScale(Vector3.one, 0.25f);
            _popedUpAnswer.DOScale(0, 0.25f).SetDelay(2f).onComplete = () =>
            _popedUpAnswer.DOScale(1, 0).onComplete = () => _popedUpAnswer.gameObject.SetActive(false);
        }
        /// <summary>
        /// Notify when round is over, and show the winner
        /// After tweening the UI, give delay, show Interstitial Ad
        /// </summary>
        public void NotifyRoundOver()
        {
            roundOvernotif.transform.localScale = Vector3.zero;
            roundOvernotif.SetActive(true);

            roundWinnerText.text = gameManager.player.scoreGain > gameManager.enemy.scoreGain ? "Player Won" : "Enemy won";

            Sequence seq = DOTween.Sequence();
            seq.Append(roundOvernotif.transform.DOScale(1, 0.25f));
            seq.AppendInterval(2f);
            seq.Append(roundOvernotif.transform.DOScale(0, 0.25f)).AppendCallback(() =>
            {
                roundOvernotif.SetActive(false);
                roundOvernotif.transform.localScale = Vector3.one;
                AdsManager.instance.ShowInterstitial();
            });
        }

        /// <summary>
        /// Showing answer, highlighting the participant's answered color.
        /// </summary>
        public void ShowAnswersUI()
        {
            answerShowPanel.transform.localScale = Vector3.zero;
            answerShowPanel.SetActive(true);

            answersTitleText.text = "Top " + gameManager.CurrentQuestion.answers.Count + " answers";

            foreach (var answer in gameManager.player.answeredDatas)
            {
                answers[answer.answerId].Display(gameManager.player);
            }

            foreach (var answer in gameManager.enemy.answeredDatas)
            {
                answers[answer.answerId].Display(gameManager.enemy);
            }

            answerShowPanel.transform.DOScale(1, 0.25f);
        }

        public void CloseAnswersUI()
        {
            answerShowPanel.transform.DOScale(0, 0.25f).onComplete = () =>
            {
                answerShowPanel.SetActive(false);
                answerShowPanel.transform.localScale = Vector3.one;
                ShowResultUI();
            };
        }

        /// <summary>
        /// Showing the result UI, can only continue after 1 second and after showing reward UI
        /// </summary>
        public void ShowResultUI()
        {

            resultUI.transform.localScale = Vector3.zero;
            resultUI.SetActive(true);

            var score = LeanPool.Spawn(scoreDisplayUIPrefab, scoreTableParent);
            score.Init(gameManager.player.scoreGain, gameManager.enemy.scoreGain);
            scoreDisplays.Add(score);

            int totalPlayerScore = 0;
            int totalEnemyScore = 0;

            foreach (var item in scoreDisplays)
            {
                totalPlayerScore += int.Parse(item.playerScoreText.text);
                totalEnemyScore += int.Parse(item.enemyScoreText.text);
            }

            playerTotalPointText.text = totalPlayerScore.ToString();            
            enemyTotalPointText.text = totalEnemyScore.ToString();
            continueButton.interactable = false;

            Sequence seq = DOTween.Sequence();
            seq.Append(resultUI.transform.DOScale(1, 0.25f));
            seq.AppendInterval(1f);
            seq.AppendCallback(() =>
            {
                ShowRewardUI();
                continueButton.interactable = true;
            });
        }


        /// <summary>
        /// The player(and the enemy too) will gain score first, after that, tween the player reward text on the top
        /// Closing reward panel UI, wether by pressing 'no thanks' button or after watching rewarded ads
        /// </summary>
        public void UpdateReward()
        {
            rewardPanel.transform.DOScale(0, 0.25f).onComplete = () =>
            {
                rewardPanel.SetActive(false);
                rewardPanel.transform.localScale = Vector3.one;
                gameManager.player.GainTotalScore();
                gameManager.enemy.GainTotalScore();
                playerRewardText.text = gameManager.player.totalScore.ToString();
                playerRewardText.transform.DOPunchScale(Vector3.one, 0.25f, 1, 1).SetDelay(0.5f);
            };
        }
        /// <summary>
        /// Closing Result UI wether it's by pressing continue button or after you watch ads
        /// Close the result UI by tween, after that tween, proceed to next round
        /// </summary>
        public void CloseResultUI()
        {
            resultUI.transform.DOScale(0, 0.25f).SetDelay(1).onComplete = () =>
            {
                resultUI.SetActive(false);
                resultUI.transform.localScale = Vector3.one;
                ResetPlayUI();

                gameManager.ProceedNextRound();
            };
        }

        /// <summary>
        /// Show reward ui, will give info about score gained and also offer button to double the score by ad
        /// </summary>
        public void ShowRewardUI()
        {
            rewardPanel.transform.localScale = Vector3.zero;
            rewardPanel.SetActive(true);
            rewardGainedText.text = gameManager.player.scoreGain.ToString();
            rewardPanel.transform.DOScale(1, 0.25f);

        }

        /// <summary>
        /// Update reward text after doubled
        /// </summary>
        public void DoublingRewardUI()
        {
            rewardGainedText.text = gameManager.player.scoreGain.ToString();
            rewardGainedText.transform.DOPunchScale(Vector3.one, 0.25f, 1, 1).SetDelay(0.5f).onComplete =() => UpdateReward();
        }
        

        /// <summary>
        /// Resetting Timer and Participants UI
        /// </summary>
        public void ResetPlayUI()
        {
            UpdateTimerUI(gameManager.gameData.timePerRound);
            UpdateParticipantUI(Participant.ControlType.Player, 0);
            UpdateParticipantUI(Participant.ControlType.Enemy, 0);
        }

        /// <summary>
        /// Showing game over UI, will show final score, who won, also a button to play again or back to menu
        /// </summary>
        public void ShowFinalResultUI()
        {
            gameOverUI.transform.localScale = Vector3.zero;
            gameOverUI.SetActive(true);
            finalWinnerText.text = gameManager.player.totalScore > gameManager.enemy.totalScore ? "Player Won!": "Enemy Won!";
            finalScoreText.text = gameManager.player.totalScore.ToString();
            gameOverUI.transform.DOScale(1, 0.25f);
        }
    }
}

