using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Rover
{
    public class Tasks : MonoBehaviour
    {
        [SerializeField] private RoverTrigger _baseTrigger;

        public bool RadarFixStarted { get; private set; }
        public bool RadarFixed { get; private set; }


        public bool SamplesCollectingStarted { get; private set; }
        public bool GreenCollected { get; private set; }
        public bool YellowCollected { get; private set; }
        public bool RedCollected { get; private set; }

        public float? MissionStartTime { get; private set; }
        public float? MissionEndTime { get; private set; }
        public float? RadarFixStartTime { get; private set; }
        public float? RadarFixEndTime { get; private set; }
        public float? SamplesCollectStartTime { get; private set; }
        public float? SamplesCollectEndTime { get; private set; }

        [System.Serializable]
        public class FailGameEvent : UnityEvent<GameFailReason> {}
        public FailGameEvent OnGameFail;
        public UnityEvent OnGameSuccess;
        public bool GameFailed { get; set; }
        private XRIDefaultInputActions _controls;

        public bool AllTasksCompleted => RadarFixed &&
            GreenCollected && YellowCollected && RedCollected;

        public GamePhase GamePhase { get; private set; }

        public static Tasks Instance { get; private set; }

        private void Start()
        {
            if (Instance)
                throw new System.InvalidOperationException("Singleton Tasks error");

            _controls = new XRIDefaultInputActions();
            _controls.Enable();

            Instance = this;
        }

        private void Update()
        {
            if (_controls.XRIButtons.Activate.ReadValue<float>() > 0.2f)
            {
                HandleActivateClick();
            }

            if (GamePhase == GamePhase.RadarFixed)
                if (GameStatistics.Instance.CollectedSamples + GameStatistics.Instance.BrokenSamples == 3)
                {
                    Instance.GamePhase = GamePhase.SamplesCollected;
                    Instance.SamplesCollectEndTime = Time.time;
                }
        }

        public void HandleActivateClick()
        {
            if (GameFailed) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public static void FailGame(GameFailReason reason)
        {
            Debug.Log(reason);
            Instance.GameFailed = true;
            Instance.OnGameFail?.Invoke(reason);
        }

        public static void HandleRadarFixStarted()
        {
            if (!Instance.RadarFixStarted)
            {
                Instance.RadarFixStarted = true;
                Instance.RadarFixStartTime = Time.time;
            }
        }

        public static void SetRadarFixed()
        {
            if (Instance.GamePhase == GamePhase.RoverTurnedOn)
            {
                Instance.RadarFixed = true;
                Instance.GamePhase = GamePhase.RadarFixed;

                Instance.RadarFixEndTime = Time.time;
            }
            else
            {
                FailGame(new InvalidAction());
            }
        }

        public static void HandleSamplesCollectingStarted()
        {
            if (!Instance.SamplesCollectingStarted)
            {
                Instance.SamplesCollectingStarted = true;
                Instance.SamplesCollectStartTime = Time.time;
            }
        }

        public static void SetGreenCollected()
        {
            if (Instance.GamePhase == GamePhase.RadarFixed)
            {
                Instance.GreenCollected = true;
                GameStatistics.Instance.CollectedSamples++;

                if (GameStatistics.Instance.CollectedSamples + GameStatistics.Instance.BrokenSamples == 3)
                {
                    Instance.GamePhase = GamePhase.SamplesCollected;
                    Instance.SamplesCollectEndTime = Time.time;
                }
            }
            else
            {
                FailGame(new InvalidAction());
            }
        }

        public static void SetYellowCollected()
        {
            if (Instance.GamePhase == GamePhase.RadarFixed)
            {
                Instance.YellowCollected = true;
                GameStatistics.Instance.CollectedSamples++;

                if (GameStatistics.Instance.CollectedSamples + GameStatistics.Instance.BrokenSamples == 3)
                {
                    Instance.GamePhase = GamePhase.SamplesCollected;
                    Instance.SamplesCollectEndTime = Time.time;
                }
            }
            else
            {
                FailGame(new InvalidAction());
            }
        }

        public static void SetRedCollected()
        {
            if (Instance.GamePhase == GamePhase.RadarFixed)
            {
                Instance.RedCollected = true;
                GameStatistics.Instance.CollectedSamples++;

                
            }
            else
            {
                FailGame(new InvalidAction());
            }
        }

        public static void HandleRoverTurnedOn()
        {
            if (Instance.GamePhase == GamePhase.BeforeStart)
            {
                Instance.GamePhase = GamePhase.RoverTurnedOn;

                Instance.MissionStartTime = Time.time;
            }
        }

        public static void HandleRoverTurnedOff()
        {
            if (Instance.GamePhase == GamePhase.SamplesCollected && Instance._baseTrigger.IsRoverTriggered)
            {
                Instance.GamePhase = GamePhase.RoverTurnedOff;
                Instance.MissionEndTime = Time.time;
                Instance.OnGameSuccess?.Invoke();

            }
        }

        public static void SetRadarPhoto(Texture2D photo)
        {
            GameStatistics.Instance.RadarPhoto = photo;
        }

        public static void SetSamplesPhoto(Texture2D photo)
        {
            GameStatistics.Instance.SamplesPhoto = photo;
        }
    }
}