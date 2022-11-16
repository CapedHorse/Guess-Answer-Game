using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PikoruaTest
{
    public class MenuManager : MonoBehaviour
    {
        public static MenuManager instance;
        public Button playButton, quitButton;
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

        private void Start()
        {
            playButton.onClick.AddListener(PlayGame);
            quitButton.onClick.AddListener(QuitGame);
        }

        public void PlayGame()
        {
            SceneManager.LoadScene("PlayScene");
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}

