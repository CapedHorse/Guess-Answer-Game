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
        public TextMeshProUGUI playerTotalPointText, enemyTotalPointText, rewardGained;
        public Button continueBtn, doubleRewardBtn;
        [Header("Countdown")]
        public GameObject countDownUI;
        public TextMeshProUGUI countDownText;

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
            answerButton.onClick.AddListener(() =>
            {
                gameManager.player.Answer(answerInputField.text);
                answerInputField.text = "";
            });

            InitQuestion();
        }

        public IEnumerator CountingDownUI(UnityAction _afterCountingDown)
        {
            countDownUI.SetActive(true);
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

        public void ParticipantAnswered(Participant _participant, string _answer)
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

