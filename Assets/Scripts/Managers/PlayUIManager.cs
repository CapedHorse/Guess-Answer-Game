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
        public Button answerButton;
        public Image timerIndicator;
        public TextMeshProUGUI answersTitleText;
        public RectTransform answersParent;
        public AnswerUI answerUIPrefab;
        public Dictionary<int, AnswerUI> answers;

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
        public TextMeshProUGUI countDownText;

        [Header("Game Over")]
        public GameObject gameOverUI;
        public TextMeshProUGUI finalStatusText, finalScoreText;
        public Button restartButton;


        public UnityAction<Participant, string> OnAnswering;

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
        
        void Start()
        {
            gameManager = GameManager.instance;

            answers = new Dictionary<int, AnswerUI>();

            Init();
        }

        private void OnEnable()
        {
            OnAnswering += ParticipantAnswered;
        }

        private void OnDisable()
        {
            OnAnswering -= ParticipantAnswered;
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
            continueButton.onClick.AddListener(() =>
            {
                //proceed next round
            });
            noThanksButton.onClick.AddListener(() =>
            {
                //close the reward gain UI
            });
            doubleRewardButton.onClick.AddListener(() =>
            {
                AdsManager.instance.ShowRewarded();
            });
            restartButton.onClick.AddListener(() =>
            {
                gameManager.RestartGame();
            });

            UpdateParticipantUI(Participant.ControlType.Player, 0);
            UpdateParticipantUI(Participant.ControlType.Enemy, 0);

        }

        public IEnumerator CountingDownUI(UnityAction _afterCountingDown)
        {
            countDownUI.SetActive(true);
            playUI.SetActive(false);
            countDownText.text = "3";
            yield return new WaitForSeconds(1);
            countDownText.text = "2";
            yield return new WaitForSeconds(1);
            countDownText.text = "1";
            yield return new WaitForSeconds(1);
            countDownText.text = "GO!";
            yield return new WaitForSeconds(1);
            countDownUI.SetActive(false);
            playUI.SetActive(true);
            InitQuestion();
            _afterCountingDown?.Invoke();
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
                    enemyAnswerPopUpText.text = _answer;
                    PopUpAnswer(enemyAnswerPopUp.transform);
                    break;
                default:
                    break;
            }
        }

        public void UpdateTimerUI()
        {
            var percentage = Mathf.Clamp((gameManager.gameData.timePerRound-gameManager.currentPlayTime) / gameManager.gameData.timePerRound * 1, 0, 1);
            timerIndicator.fillAmount = Mathf.Clamp(percentage,0, 1);
        }

        public void UpdateParticipantUI(Participant.ControlType type, int value)
        {
            switch (type)
            {
                case Participant.ControlType.Player:
                    playerScoreFill.fillAmount = Mathf.Clamp((float) value / 100 * 1, 0, 1);
                    playerScoreText.text = value.ToString();
                    break;
                case Participant.ControlType.Enemy:
                    enemyScoreFill.fillAmount = Mathf.Clamp((float)value / 100 * 1, 0, 1);
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

        public void ShowResultUI()
        {
            
        }
        
        void Update()
        {

        }
    }
}

