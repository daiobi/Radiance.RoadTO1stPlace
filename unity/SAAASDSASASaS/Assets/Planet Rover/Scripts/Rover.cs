using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rover
{
    [RequireComponent(typeof(RoverMovement), typeof(RoverHealth), typeof(RoverBattery))]
    public class Rover : MonoBehaviour
    {
        private RoverMovement _roverMovement;
        private RoverHealth _roverHealth;
        private RoverBattery _roverBattery;

        private bool _isActivated = false;

        private void Awake()
        {
            _roverMovement = GetComponent<RoverMovement>();
            _roverHealth = GetComponent<RoverHealth>();
            _roverBattery = GetComponent<RoverBattery>();

            TurnOff();
        }

        public void TurnOn()
        {
            _roverMovement.enabled = true;
            _roverMovement.enabled = true;
            _roverBattery.enabled = true;
            _isActivated = true;
        }

        public void TurnOff()
        {
            _roverMovement.Move(0, 0);
            _roverMovement.enabled = false;
            _roverHealth.enabled = false;
            _roverBattery.enabled = false;
            _isActivated = false;
        }

        public void Move(float acceleration, float steering)
        {
            if (_isActivated)
            {
                _roverMovement.Move(acceleration, steering);
            }
        }

        public void RepairWheel(int n)
        {
            if (_isActivated)
            {
                _roverHealth.RepairWheel(n);
            }
        }

        public Telemetry GetTelemetry()
        {
            return new Telemetry()
            {
                Position = transform.position,
                Direction = transform.rotation.eulerAngles.y,
                HorizontalAngle = Vector3.Angle(transform.up, Vector3.up),
                Battery = _roverBattery.Value,
                Speed = _roverMovement.SpeedKmPH,

                HitCount = _roverHealth.HitCount,
                BodyBroken = _roverHealth.IsBodyBroken,
                LFBroken = _roverHealth.IsLFWheelBroken,
                RFBroken = _roverHealth.IsRFWheelBroken,
                LCBroken = _roverHealth.IsLCWheelBroken,
                RCBroken = _roverHealth.IsRCWheelBroken,
                LBBroken = _roverHealth.IsLBWheelBroken,
                RBBroken = _roverHealth.IsRBWheelBroken,
            };
        }
    }
}