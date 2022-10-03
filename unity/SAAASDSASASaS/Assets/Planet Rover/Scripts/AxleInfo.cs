using UnityEngine;

namespace Rover
{
    [System.Serializable]
    public struct AxleInfo
    {
        public WheelCollider leftWheel;
        public WheelCollider rightWheel;
        public bool motor;
        public bool steering;
        public bool invertSteering;
    }
}