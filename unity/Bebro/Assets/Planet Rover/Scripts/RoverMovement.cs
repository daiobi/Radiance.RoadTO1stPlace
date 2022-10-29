using UnityEngine;

namespace Rover
{
    public class RoverMovement : MonoBehaviour
    {
        [SerializeField] private float _maxSpeed;
        [Header("Wheels")]
        [SerializeField] private AxleInfo[] _axles;

        private Vector3 _lastPosition;
        private float _currentSpeed;

        public float SpeedKmPH => _currentSpeed * 3.6f;

        private void FixedUpdate()
        {
            _currentSpeed = (transform.position - _lastPosition).magnitude / Time.fixedDeltaTime;
            _lastPosition = transform.position;

            if (SpeedKmPH > _maxSpeed)
            {
                GameStatistics.Instance.FullMaxSpeedTime += Time.fixedDeltaTime;
                GameStatistics.Instance.MaxSpeedTime += Time.fixedDeltaTime;
            }
            else
            {
                GameStatistics.Instance.MaxSpeedTime = 0;
            }
        }

        public void Move(float acceleration, float steering)
        {
            foreach (AxleInfo axleInfo in _axles)
            {
                if (axleInfo.steering)
                {
                    axleInfo.leftWheel.SetSteering(axleInfo.invertSteering ? -steering : steering);
                    axleInfo.rightWheel.SetSteering(axleInfo.invertSteering ? -steering : steering);
                }
                if (axleInfo.motor)
                {
                    axleInfo.leftWheel.SetTorque(acceleration);
                    axleInfo.rightWheel.SetTorque(acceleration);
                }
            }
        }
    }
}