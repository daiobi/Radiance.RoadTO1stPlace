using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rover
{
    public class GameStatistics : MonoBehaviour
    {
        public static GameStatistics Instance { get; private set; }

        public float MaxSpeedTime { get; set; }

        private void Awake()
        {
            if (Instance != null)
            {
                throw new System.InvalidOperationException($"{GetType().Name} singleton error");
            }

            Instance = this;
        }
    }
}