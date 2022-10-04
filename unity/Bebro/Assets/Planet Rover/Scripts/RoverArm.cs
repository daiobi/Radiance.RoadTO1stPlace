using UnityEngine;

namespace Rover
{
    public class RoverArm : MonoBehaviour
    {
        [SerializeField] private Transform _axis;
        [SerializeField] private Transform _target;
        [SerializeField] private float _xSpeed;
        [SerializeField] private float _ySpeed;
        [SerializeField] private float _zSpeed;

        public void Move(float x, float y, float z)
        {
            _axis.Rotate(0, x * _xSpeed * Time.deltaTime, 0);
            _target.Translate(0, y * _ySpeed * Time.deltaTime, -z * _zSpeed * Time.deltaTime, Space.Self);
        }
    }
}
