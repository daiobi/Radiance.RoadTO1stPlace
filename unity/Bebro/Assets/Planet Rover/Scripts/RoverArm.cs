using System.Collections;
using UnityEngine;

namespace Rover
{
    public class RoverArm : MonoBehaviour
    {
        [SerializeField] private float _maxDistance;
        [SerializeField] private PhysicalRotator _axis;
        [SerializeField] private Transform _armPosition;
        [SerializeField] private Transform _target;
        [SerializeField] private float _xSpeed;
        [SerializeField] private float _ySpeed;
        [SerializeField] private float _zSpeed;

        private bool _enableMovement = true;
        private float _axisAngle = 0;

        private void Update()
        {
            _axis.SetTargetAngle(_axisAngle);
        }

        public void Move(float x, float y, float z)
        {
            if (!_enableMovement) return;

            _axisAngle += x * _xSpeed * Time.deltaTime;
            if (_axisAngle > 180) _axisAngle = _axisAngle - 360;
            if (_axisAngle < -180) _axisAngle = 360 + _axisAngle;

            Vector3 offset = new Vector3(0, y * _ySpeed * Time.deltaTime, -z * _zSpeed * Time.deltaTime);
            float distance = Vector3.Distance(_target.position + offset, _axis.transform.position);
            if (distance <= _maxDistance)
            {
                _target.Translate(offset, Space.Self);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(_axis.transform.position, _maxDistance);
        }
    }
}
