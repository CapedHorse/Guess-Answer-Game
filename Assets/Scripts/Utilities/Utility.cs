using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PikoruaTest
{
    public class Utility
    {
        /// <summary>
        /// Boilerplate to create singleton instance
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="toBeInstance"></param>

        public static void CreateInstance(Component instance, Component toBeInstance)
        {
            if (instance == null)
            {
                
                instance = toBeInstance;
                
            }
            else
            {
                Debug.Log("Instance");
                MonoBehaviour.Destroy(toBeInstance.gameObject);
                return;
            }
        }
    }
}


