using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PikoruaTest
{
    public class MainManager : MonoBehaviour
    {
        public static MainManager instance;

        public GameData generalProperties;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
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

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

