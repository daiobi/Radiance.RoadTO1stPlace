using UnityEngine;
using UnityEngine.Events;

namespace Rover
{
    [RequireComponent(typeof(RoverMovement), typeof(RoverHealth), typeof(RoverBattery))]
    [RequireComponent(typeof(RoverBoxes), typeof(RoverArm))]
    public class Rover : MonoBehaviour
    {
        [SerializeField] private int _maxHealth;

        private RoverMovement _roverMovement;
        private RoverHealth _roverHealth;
        private RoverBattery _roverBattery;
        private RoverBoxes _roverBoxes;
        private RoverArm _roverArm;
        public bool IsActivated { get; private set; } = false;

        public enum BreakDownCause
        {
            BatteryLow,
            Flip,
            Health,
            BoxInvalid,
        }
        public class RoverEvent : UnityEvent<BreakDownCause> { }
        public RoverEvent OnBroken;
        private bool _isBroken;


        private void Awake()
        {
            _roverMovement = GetComponent<RoverMovement>();
            _roverHealth = GetComponent<RoverHealth>();
            _roverBattery = GetComponent<RoverBattery>();
            _roverBoxes = GetComponent<RoverBoxes>();
            _roverArm = GetComponent<RoverArm>();
        }

        private void Update()
        {
            if (_isBroken) return;

            if (_roverBattery.Value == 0)
            {
                _isBroken = true;
                OnBroken?.Invoke(BreakDownCause.BatteryLow);
            }
            else if (Vector3.Angle(transform.up, Vector3.up) > 90)
            {
                _isBroken = true;
                OnBroken?.Invoke(BreakDownCause.Flip);
            }
            else if (_roverHealth.HitCount > _maxHealth)
            {
                _isBroken = true;
                OnBroken?.Invoke(BreakDownCause.Health);
            }
            else if (_roverBoxes.GreenState == BoxState.FilledInvalid ||
                _roverBoxes.YellowState == BoxState.FilledInvalid ||
                _roverBoxes.RedState == BoxState.FilledInvalid)
            {
                _isBroken = true;
                OnBroken?.Invoke(BreakDownCause.BoxInvalid);
            }

            if (_roverBoxes.GreenState == BoxState.Filled) Tasks.SetGreenCollected();
            if (_roverBoxes.YellowState == BoxState.Filled) Tasks.SetYellowCollected();
            if (_roverBoxes.RedState == BoxState.Filled) Tasks.SetRedCollected();

            if (_isBroken)
            {
                TurnOff();
                Debug.Log("Broken");
            }
        }

        public void RepairWheel(int n)
        {
            if (IsActivated)
            {
                _roverHealth.RepairWheel(n);
            }
        }
        public bool TurnOn()
        {
            if (_isBroken) return false;

            _roverMovement.enabled = true;
            _roverHealth.enabled = true;
            _roverBattery.enabled = true;
            _roverBoxes.enabled = true;
            _roverArm.enabled = true;
            IsActivated = true;

            return true;
        }
        public void TurnOff()
        {
            _roverMovement.Move(0, 0);
            _roverArm.Move(0, 0, 0);
            _roverMovement.enabled = false;
            _roverHealth.enabled = false;
            _roverBattery.enabled = false;
            _roverBoxes.enabled = false;
            _roverArm.enabled = false;
            IsActivated = false;
        }
        public void Move(float acceleration, float steering)
        {
            if (IsActivated)
            {
                _roverMovement.Move(acceleration, steering);
            }
        }
        public void MoveArm(float x, float y, float z)
        {
            if (IsActivated)
            {
                _roverArm.Move(x, y, z);
            }
        }
        public void SetArmGrab(float a)
        {
            if (IsActivated)
            {
                _roverArm.SetGrab(a);
            }
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
        public void OpenBlueBox()
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
            return new Telemetry()
            {
                Position = transform.position,
                Direction = transform.rotation.eulerAngles.y,
                HorizontalAngle = Vector3.Angle(transform.up, Vector3.up),
                BatteryPercents = _roverBattery.ValuePercents,
                Speed = _roverMovement.SpeedKmPH,

                Health = _maxHealth - _roverHealth.HitCount,
                BodyBroken = _roverHealth.IsBodyBroken,
                LFBroken = _roverHealth.IsLFWheelBroken,
                RFBroken = _roverHealth.IsRFWheelBroken,
                LCBroken = _roverHealth.IsLCWheelBroken,
                RCBroken = _roverHealth.IsRCWheelBroken,
                LBBroken = _roverHealth.IsLBWheelBroken,
                RBBroken = _roverHealth.IsRBWheelBroken,

                greenBoxState = _roverBoxes.GreenState,
                yellowBoxState = _roverBoxes.YellowState,
                blueBoxState = _roverBoxes.RedState,
            };
        }
    }
}