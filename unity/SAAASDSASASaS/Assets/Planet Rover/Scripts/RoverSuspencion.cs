using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rover
{
    public class RoverSuspencion : MonoBehaviour
    {
        [SerializeField]
        private WheelCollider _forwardLeft;
        [SerializeField]
        private Transform _forwardLeftVisual;
        [SerializeField]
        private WheelCollider _centerLeft;
        [SerializeField]
        private Transform _centerLeftVisual;
        [SerializeField]
        private WheelCollider _backwardLeft;
        [SerializeField]
        private Transform _backwardLeftVisual;

        [SerializeField]
        private WheelCollider _forwardRight;
        [SerializeField]
        private Transform _forwardRightVisual;
        [SerializeField]
        private WheelCollider _centerRight;
        [SerializeField]
        private Transform _centerRightVisual;
        [SerializeField]
        private WheelCollider _backwardRight;
        [SerializeField]
        private Transform _backwardRightVisual;

        [SerializeField]
        private Transform _leftAxle23;
        [SerializeField]
        private Transform _leftAxle123;
        [SerializeField]
        private Transform _rightAxle23;
        [SerializeField]
        private Transform _rightAxle123;


        private Vector3 _forwardLeftPosition;
        private Vector3 _forwardRightPosition;
        private Vector3 _centerLeftPosition;
        private Vector3 _centerRightPosition;
        private Vector3 _backwardLeftPosition;
        private Vector3 _backwardRightPosition;

        private Quaternion _forwardLeftRotation;
        private Quaternion _forwardRightRotation;
        private Quaternion _centerLeftRotation;
        private Quaternion _centerRightRotation;
        private Quaternion _backwardLeftRotation;
        private Quaternion _backwardRightRotation;

        private void Update()
        {
            _forwardLeft.GetWorldPose(out _forwardLeftPosition, out _forwardLeftRotation);
            _forwardRight.GetWorldPose(out _forwardRightPosition, out _forwardRightRotation);
            _centerLeft.GetWorldPose(out _centerLeftPosition, out _centerLeftRotation);
            _centerRight.GetWorldPose(out _centerRightPosition, out _centerRightRotation);
            _backwardLeft.GetWorldPose(out _backwardLeftPosition, out _backwardLeftRotation);
            _backwardRight.GetWorldPose(out _backwardRightPosition, out _backwardRightRotation);


            Vector3 ldir23 = (_backwardLeftPosition - _centerLeftPosition).normalized;
            Vector3 lmid23 = (_centerLeftPosition + _backwardLeftPosition) * 0.5f;

            //_leftAxle23.position = lmid23;
            _leftAxle23.rotation = Quaternion.LookRotation(-ldir23);

            Vector3 ldir123 = (_forwardLeftPosition - lmid23).normalized;
            _leftAxle123.rotation = Quaternion.LookRotation(ldir123);


            Vector3 rdir23 = (_backwardRightPosition - _centerRightPosition).normalized;
            Vector3 rmid23 = (_centerRightPosition + _backwardRightPosition) * 0.5f;

            //_rightAxle23.position = rmid23;
            _rightAxle23.rotation = Quaternion.LookRotation(-rdir23);

            Vector3 rdir123 = (_forwardRightPosition - rmid23).normalized;
            _rightAxle123.rotation = Quaternion.LookRotation(rdir123);

            _forwardLeftVisual.rotation = _forwardLeftRotation;
            _forwardRightVisual.rotation = _forwardRightRotation;
            _centerLeftVisual.rotation = _centerLeftRotation;
            _centerRightVisual.rotation = _centerRightRotation;
            _backwardLeftVisual.rotation = _backwardLeftRotation;
            _backwardRightVisual.rotation = _backwardRightRotation;
        }
    }
}