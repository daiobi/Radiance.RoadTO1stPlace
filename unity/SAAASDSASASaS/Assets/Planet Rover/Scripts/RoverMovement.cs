using UnityEngine;

namespace Rover
{
    public class RoverMovement : MonoBehaviour
    {
        [Header("Wheels")]
        [SerializeField]
        private AxleInfo[] _axles;
        [Header("Settings")]
        [SerializeField]
        private float _maxSteering = 45f;
        [SerializeField]
        private float _maxTorgue = 25f;
        [SerializeField]
        private float _maxBrake = 50f;

        private Vector3 _lastPosition;
        private float _currentSpeed;

        private void FixedUpdate()
        {
            _currentSpeed = (transform.position - _lastPosition).magnitude / Time.fixedDeltaTime;
            _lastPosition = transform.position;

            Debug.Log(_currentSpeed);

        }

        public void Move(float acceleration, float steering)
        {
            foreach (AxleInfo axleInfo in _axles)
            {
                if (axleInfo.steering)
                {
                    axleInfo.leftWheel.steerAngle = _maxSteering * (axleInfo.invertSteering ? -steering : steering);
                    axleInfo.rightWheel.steerAngle = _maxSteering * (axleInfo.invertSteering ? -steering : steering);
                }
                if (axleInfo.motor)
                {
                    axleInfo.leftWheel.brakeTorque = acceleration == 0 ? _maxBrake : 0f;
                    axleInfo.rightWheel.brakeTorque = acceleration == 0 ? _maxBrake : 0f;

                    axleInfo.leftWheel.motorTorque = acceleration * _maxTorgue;
                    axleInfo.rightWheel.motorTorque = acceleration * _maxTorgue;
                }
            }
        }
    }
}