using UnityEngine;
using UnityEngine.Events;

namespace Rover
{
    [RequireComponent(typeof(RoverMovement), typeof(RoverHealth), typeof(RoverBattery))]
    [RequireComponent(typeof(RoverBoxes))]
    public class Rover : MonoBehaviour
    {
        [SerializeField] private RoverArm _grabArm;
        [SerializeField] private GrabHandTool _grabTool;

        [SerializeField] private RoverArm _drillArm;
        [SerializeField] private DrillTool _drillTool;

        [SerializeField] private SignalController _worldCenter;
        [SerializeField] private float _maxSpeed = 15f;
        [SerializeField] private int _maxHealth;

        private RoverMovement _roverMovement;
        private RoverHealth _roverHealth;
        private RoverBattery _roverBattery;
        private RoverBoxes _roverBoxes;
        public bool IsActivated { get; private set; } = false;

        public enum BreakDownCause
        {
            BatteryLow,
            Flip,
            Health,
            Distance,
        }
        [System.Serializable]
        public class RoverEvent : UnityEvent<BreakDownCause> { }
        public RoverEvent OnBroken;
        private bool _isBroken;

        private void Awake()
        {
            _roverMovement = GetComponent<RoverMovement>();
            _roverHealth = GetComponent<RoverHealth>();
            _roverBattery = GetComponent<RoverBattery>();
            _roverBoxes = GetComponent<RoverBoxes>();
        }

        private void Update()
        {
            if (!IsActivated || _isBroken || Tasks.Instance.GamePhase == GamePhase.RoverTurnedOff) return;

            _roverMovement.MaxSpeed = _maxSpeed + 1 - 2 * _roverHealth.HitCount;

            BreakDownCause cause = BreakDownCause.BatteryLow;
            if (_roverBattery.Value == 0)
            {
                _isBroken = true;
                cause = BreakDownCause.BatteryLow;
            }
            else if (GetHealth() <= 0)
            {
                _isBroken = true;
                cause = BreakDownCause.Health;
            }
            else if (GetSignalLevel() < 0.05f)
            {
                _isBroken = true;
                cause = BreakDownCause.Distance;
            }

            if (_isBroken)
            {
                OnBroken?.Invoke(cause);
                Tasks.FailGame(new RoverBrokenDown(cause));
                TurnOff();
                Debug.Log("Broken");
            }

            if (Vector3.Angle(transform.up, Vector3.up) > 90)
            {
                _roverHealth.TakeDamage(_maxHealth);
            }

            if (GameStatistics.Instance.MaxSpeedTime > 30f)
            {
                GameStatistics.Instance.MaxSpeedTime = 0f;
                GameStatistics.Instance.RegisterEvent(new MaxSpeedBrokenRecord());
                _roverHealth.TakeDamage(1);
            } 
        }

        public bool RepairWheel(int n)
        {
            if (IsActivated)
            {
                return _roverHealth.RepairWheel(n);
            }

            return false;
        }

        public bool TurnOn()
        {
            if (_isBroken) return false;

            _roverMovement.enabled = true;
            _roverHealth.enabled = true;
            _roverBattery.enabled = true;
            _roverBoxes.enabled = true;
            _grabArm.enabled = true;
            _drillArm.enabled = true;
            IsActivated = true;

            Tasks.HandleRoverTurnedOn();

            return true;
        }
        public void TurnOff()
        {

            _roverMovement.Torque = 0;
            _roverMovement.Steering = 0;

            _grabArm.Move(0, 0, 0);
            _drillArm.Move(0, 0, 0);
            _roverMovement.enabled = false;
            _roverHealth.enabled = false;
            _roverBattery.enabled = false;
            _roverBoxes.enabled = false;
            _grabArm.enabled = false;
            _drillArm.enabled = false;
            IsActivated = false;

            Tasks.HandleRoverTurnedOff();
        }
        public void Move(float acceleration, float steering)
        {
            if (IsActivated)
            {
                _roverMovement.Torque = acceleration;
                _roverMovement.Steering = steering;
            }
        }
        public void MoveGrabArm(float x, float y, float z)
        {
            if (IsActivated)
            {
                _grabArm.Move(x, y, z);
            }
        }

        public void MoveDrillArm(float x, float y, float z)
        {
            if (IsActivated)
            {
                _drillArm.Move(x, y, z);
            }
        }

        public void SetArmGrab(float a)
        {
            if (IsActivated)
            {
                _grabTool.SetGrabValue(a);
            }
        }

        public void SetDrillSpeed(float a)
        {
            if (IsActivated)
            {
                _drillTool.SetActivated(a > 0.2f);
            }
        }

        public void SetGrabArmActive(bool state)
        {
            _grabArm.SetActive(state);
        }

        public void SetDrillArmActive(bool state)
        {
            _drillArm.SetActive(state);
        }

        public void OpenGreenBox()
        {
            if (IsActivated)
            {
                _roverBoxes.OpenGreen();
            }
        }
        public void OpenYellowBox()
        {
            if (IsActivated)
            {
                _roverBoxes.OpenYellow();
            }
        }
        public void OpenRedBox()
        {
            if (IsActivated)
            {
                _roverBoxes.OpenRed();
            }
        }
        public void CloseBoxes()
        {
            _roverBoxes.CloseAll();
        }

        public Telemetry GetTelemetry()
        {
            float batteryPercents = _roverBattery.ValuePercents;
            float speed = _roverMovement.SpeedKmPH;
            bool lfBroken = _roverHealth.IsLFWheelBroken;
            bool lcBroken = _roverHealth.IsLCWheelBroken;
            bool lbBroken = _roverHealth.IsLBWheelBroken;
            bool rfBroken = _roverHealth.IsRFWheelBroken;
            bool rcBroken = _roverHealth.IsRCWheelBroken;
            bool rbBroken = _roverHealth.IsRBWheelBroken;

            return new Telemetry()
            {
                Position = transform.position,
                Direction = transform.rotation.eulerAngles.y,
                HorizontalAngle = Vector3.Angle(transform.up, Vector3.up),
                BatteryPercents = batteryPercents,
                Speed = speed,
                Signal = GetSignalLevel(),

                Health = GetHealth(),
                LFBroken = lfBroken,
                RFBroken = rfBroken,
                LCBroken = lcBroken,
                RCBroken = rcBroken,
                LBBroken = lbBroken,
                RBBroken = rbBroken,
            };
        }

        private int GetHealth()
        {
            return Mathf.Max(0, _maxHealth - _roverHealth.HitCount);
        }

        private float GetSignalLevel()
        {
            return Mathf.Clamp01(_worldCenter.GetSignalValue(
                    Vector3.Distance(transform.position, _worldCenter.transform.position)
                    )
                );
        }
    }
}