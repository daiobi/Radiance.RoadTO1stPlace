using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rover
{
    public class Tasks : MonoBehaviour
    {
        public bool RadarFixed { get; private set; }
        public bool GreenCollected { get; private set; }

        public bool YellowCollected { get; private set; }
        public bool RedCollected { get; private set; }

        public static bool AllTasksCompleted => _instance.RadarFixed &&
            _instance.GreenCollected && _instance.YellowCollected && _instance.RedCollected;

        private static Tasks _instance;

        private void Start()
        {
            if (_instance)
                throw new System.InvalidOperationException("Singleton Tasks error");

            _instance = this;
        }

        public static void SetRadarFixed()
        {
            _instance.RadarFixed = true;
        }

        public static void SetGreenCollected()
        {
            _instance.GreenCollected = true;
        }

        public static void SetYellowCollected()
        {
            _instance.YellowCollected = true;
        }

        public static void SetRedCollected()
        {
            _instance.RedCollected = true;
        }
    }
}