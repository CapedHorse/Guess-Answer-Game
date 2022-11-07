using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PikoruaTest
{
    public class PlayUIManager : MonoBehaviour
    {
        public static PlayUIManager instance;

        public Canvas playCanvas;

        [Header("Gameplay")]
        public TextMeshProUGUI questionText;
        public Image playerScoreFill, enemyScoreFill;
        public Image playerScoreBg, enemyScoreBg;
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
        public GameObject notifParent;

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
            Debug.Log(GameManager.instance == null);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

