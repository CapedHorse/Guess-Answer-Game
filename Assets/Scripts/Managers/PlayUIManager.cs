using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PikoruaTest
{
    public class PlayUIManager : MonoBehaviour
    {
        public static PlayUIManager instance;
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

