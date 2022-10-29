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

        private float _missionStartTime = 0f;
        public float MissionStartTime { get => _missionStartTime == 0f ? Time.time : _missionStartTime; }

        private float _missionEndTime = 0f;
        public float MissionEndTime { get => _missionEndTime == 0f ? Time.time : _missionEndTime; }

        private float _radarFixStartTime = 0f;
        public float RadarFixStartTime { get => _radarFixStartTime == 0f ? Time.time : _radarFixStartTime; }

        private float _radarFixEndTime = 0f;
        public float RadarFixEndTime { get => _radarFixEndTime == 0f ? Time.time : _radarFixEndTime; }

        private float _samplesCollectStartTime = 0f;
        public float SamplesCollectStartTime { get => _samplesCollectStartTime == 0f ? Time.time : _samplesCollectStartTime; }

        private float _samplesCollectEndTime = 0f;
        public float SamplesCollectEndTime { get => _samplesCollectEndTime == 0f ? Time.time : _samplesCollectEndTime; }

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

#if UNITY_EDITOR
            //GamePhase = GamePhase.RadarFixed;
#endif
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
                Instance._radarFixStartTime = Time.time;
            }
        }

        public static void SetRadarFixed()
        {
            if (Instance.GamePhase == GamePhase.RoverTurnedOn)
            {
                Instance._radarFixedCheck.SetActive(true);
                Instance.RadarFixed = true;
                Instance.GamePhase = GamePhase.RadarFixed;

                Instance._radarFixEndTime = Time.time;
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
                Instance._samplesCollectStartTime = Time.time;
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
                    Instance._samplesCollectEndTime = Time.time;
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
                    Instance._samplesCollectEndTime = Time.time;
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
                    Instance._samplesCollectEndTime = Time.time;
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

                Instance._missionStartTime = Time.time;
            }
        }

        public static void HandleRoverTurnedOff()
        {
            if (Instance.GamePhase == GamePhase.SamplesCollected && Instance._baseTrigger.IsRoverTriggered)
            {
                Instance._roverDeactivatedCheck.SetActive(true);
                Instance.GamePhase = GamePhase.RoverTurnedOff;
                Instance.OnGameSuccess?.Invoke();

                Instance._missionEndTime = Time.time;
            }
        }
    }
}