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
        public TMP_InputField answerInputField;
        public Button answerButton;
        public Image timerIndicator;
        public TextMeshProUGUI answersTitleText;
        public RectTransform answersParent;
        public AnswerUI answerUIPrefab;
        public List<AnswerUI> answers;

        [Header("Notifications")]
        public GameObject notifUI;

        [Header("Countdown")]
        public GameObject countDownUI;
        public TextMeshProUGUI countDownText;

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


        }

        /// <summary>
        /// Initiate the UI
        /// </summary>
        void Init()
        {
            
            
        }

        public IEnumerator CountingDownUI(UnityAction afterCountingDown)
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
            afterCountingDown?.Invoke();
        }
        
        void Update()
        {

        }
    }
}

