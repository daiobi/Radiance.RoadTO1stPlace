using System;
using UnityEngine;

namespace Rover
{
    [RequireComponent(typeof(HingeJoint))]
    public class PhysicalRotator : MonoBehaviour
    {
        [SerializeField] private float _torque;

        private HingeJoint _joint;

        private void Awake()
        {
            _joint = GetComponent<HingeJoint>();
        }

        public void SetTargetAngle(float angle)
        {
            _joint.spring = new JointSpring() { spring = _joint.spring.spring, damper = _joint.spring.damper, targetPosition = angle };
        }
    }
}