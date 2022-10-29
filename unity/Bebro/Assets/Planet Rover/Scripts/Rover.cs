﻿using UnityEngine;
using UnityEngine.Events;

namespace Rover
{
    [RequireComponent(typeof(RoverMovement), typeof(RoverHealth), typeof(RoverBattery))]
    [RequireComponent(typeof(RoverBoxes), typeof(RoverArm))]
    public class Rover : MonoBehaviour
    {
        [SerializeField] private SignalController _worldCenter;
        [SerializeField] private float _maxSpeed = 15f;
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
            _roverArm = GetComponent<RoverArm>();
        }

        private void Update()
        {
            if (!IsActivated || _isBroken || Tasks.Instance.GamePhase == GamePhase.RoverTurnedOff) return;

            _roverMovement.MaxSpeed = _maxSpeed - 2 * _roverHealth.HitCount;

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
            _roverArm.enabled = true;
            IsActivated = true;

            Tasks.HandleRoverTurnedOn();

            return true;
        }
        public void TurnOff()
        {
            _roverMovement.Torque = 0;
            _roverMovement.Steering = 0;

            _roverArm.Move(0, 0, 0);
            _roverMovement.enabled = false;
            _roverHealth.enabled = false;
            _roverBattery.enabled = false;
            _roverBoxes.enabled = false;
            _roverArm.enabled = false;
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

        public void SetArmActive(bool state)
        {
            _roverArm.SetActive(state);
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
                Signal = GetSignalLevel(),

                Health = GetHealth(),
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