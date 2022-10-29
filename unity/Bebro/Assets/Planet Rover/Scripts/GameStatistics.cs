using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rover
{
    public class GameStatistics : MonoBehaviour
    {
        public static GameStatistics Instance { get; private set; }

        public float MissionTime => Tasks.Instance.MissionEndTime - Tasks.Instance.MissionStartTime;
        public float StartToRadarTime => Tasks.Instance.RadarFixStartTime - Tasks.Instance.MissionStartTime;
        public float RadarFixTime => Tasks.Instance.RadarFixEndTime - Tasks.Instance.RadarFixStartTime;
        public float RadarToSamplesTime => Tasks.Instance.RadarFixEndTime - Tasks.Instance.SamplesCollectStartTime;
        public float SamplesCollectingTime => Tasks.Instance.SamplesCollectEndTime - Tasks.Instance.SamplesCollectStartTime;
        public float SamplesToEndTime => Tasks.Instance.SamplesCollectStartTime - Tasks.Instance.SamplesCollectEndTime;
        public float MaxSpeedTime { get; set; }
        public float FullMaxSpeedTime { get; set; }
        
        public int CollectedSamples { get; set; }
        public int BrokenSamples { get; set; }

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