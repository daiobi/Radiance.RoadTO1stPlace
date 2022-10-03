using UnityEngine;

namespace Rover
{
    public class RoverMovement : MonoBehaviour
    {
        [Header("Wheels")]
        [SerializeField]
        private WheelCollider _forwardLeft;
        [SerializeField]
        private WheelCollider _forwardRight;
        [SerializeField]
        private WheelCollider _centerLeft;
        [SerializeField]
        private WheelCollider _centerRight;
        [SerializeField]
        private WheelCollider _backwardLeft;
        [SerializeField]
        private WheelCollider _backwardRight;
        [Header("Settings")]
        [SerializeField]
        private float maxSteer = 45f;

        public void Move(float acceleration, float steering)
        {
            _forwardRight.steerAngle = steering * maxSteer;
            _forwardLeft.steerAngle = steering * maxSteer;
            _backwardRight.steerAngle = -steering * maxSteer;
            _backwardLeft.steerAngle = -steering * maxSteer;
        }
    }
}