using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Rover
{
    public class Tasks : MonoBehaviour
    {
        [SerializeField] private RoverTrigger _baseTrigger;
        [SerializeField] private GameObject _roverActivatedCheck;
        [SerializeField] private GameObject _radarFixedCheck;
        [SerializeField] private GameObject _greenSampleCollectedCheck;
        [SerializeField] private GameObject _yellowSampleCollectedCheck;
        [SerializeField] private GameObject _redSampleCollectedCheck;
        [SerializeField] private GameObject _roverDeactivatedCheck;

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
        private bool _gameFailed;
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

            _roverActivatedCheck.SetActive(false);
            _radarFixedCheck.SetActive(false);
            _greenSampleCollectedCheck.SetActive(false);
            _yellowSampleCollectedCheck.SetActive(false);
            _redSampleCollectedCheck.SetActive(false);
            _roverDeactivatedCheck.SetActive(false);
        }

        private void Update()
        {
            if (_controls.XRIButtons.Activate.ReadValue<float>() > 0.2f)
            {
                HandleActivateClick();
            }
        }

        public void HandleActivateClick()
        {
            if (_gameFailed) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public static void FailGame(GameFailReason reason)
        {
            Debug.Log(reason);
            Instance.OnGameFail?.Invoke(reason);
            Instance._gameFailed = true;
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
                Instance._radarFixedCheck.SetActive(true);
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
                Instance._greenSampleCollectedCheck.SetActive(true);
                Instance.GreenCollected = true;
                GameStatistics.Instance.CollectedSamples++;

                if (Instance.GreenCollected && Instance.YellowCollected && Instance.RedCollected)
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
                Instance._yellowSampleCollectedCheck.SetActive(true);
                Instance.YellowCollected = true;
                GameStatistics.Instance.CollectedSamples++;

                if (Instance.GreenCollected && Instance.YellowCollected && Instance.RedCollected)
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
                Instance._redSampleCollectedCheck.SetActive(true);
                Instance.RedCollected = true;
                GameStatistics.Instance.CollectedSamples++;

                if (Instance.GreenCollected && Instance.YellowCollected && Instance.RedCollected)
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

        public static void HandleRoverTurnedOn()
        {
            if (Instance.GamePhase == GamePhase.BeforeStart)
            {
                Instance._roverActivatedCheck.SetActive(true);
                Instance.GamePhase = GamePhase.RoverTurnedOn;

                Instance.MissionStartTime = Time.time;
            }
        }

        public static void HandleRoverTurnedOff()
        {
            if (Instance.GamePhase == GamePhase.SamplesCollected && Instance._baseTrigger.IsRoverTriggered)
            {
                Instance._roverDeactivatedCheck.SetActive(true);
                Instance.GamePhase = GamePhase.RoverTurnedOff;
                Instance.OnGameSuccess?.Invoke();

                Instance.MissionEndTime = Time.time;
            }
        }
    }
}