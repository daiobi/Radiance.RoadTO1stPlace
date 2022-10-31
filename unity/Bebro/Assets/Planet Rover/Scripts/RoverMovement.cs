using UnityEngine;

namespace Rover
{
    public class RoverMovement : MonoBehaviour
    {
        [Header("Wheels")]
        [SerializeField] private AxleInfo[] _axles;

        private Vector3 _lastPosition;
        private float _currentSpeed;


        public float MaxSpeed { get; set; }
        public float Torque { get; set; }
        public float Steering { get; set; }
        public float SpeedKmPH => _currentSpeed * 3.6f;

        private void FixedUpdate()
        {
            _currentSpeed = (transform.position - _lastPosition).magnitude / Time.fixedDeltaTime;
            _lastPosition = transform.position;

            if (MaxSpeed - SpeedKmPH < 1f)
            {
                GameStatistics.Instance.FullMaxSpeedTime += Time.fixedDeltaTime;
                GameStatistics.Instance.MaxSpeedTime += Time.fixedDeltaTime;
            }
            else
            {
                GameStatistics.Instance.MaxSpeedTime = 0;
            }

            foreach (AxleInfo axleInfo in _axles)
            {
                if (axleInfo.motor)
                {
                    axleInfo.leftWheel.Torque = SpeedKmPH < MaxSpeed ? Torque : 0;
                    axleInfo.rightWheel.Torque = SpeedKmPH < MaxSpeed ? Torque : 0;
                }
            }

            foreach (AxleInfo axleInfo in _axles)
            {
                if (axleInfo.steering)
                {
                    axleInfo.leftWheel.SetSteering(axleInfo.invertSteering ? -Steering : Steering);
                    axleInfo.rightWheel.SetSteering(axleInfo.invertSteering ? -Steering : Steering);
                }
            }
        }
    }
}