using UnityEngine;

namespace Rover
{
    public class RoverSuspencion : MonoBehaviour
    {
        [SerializeField]
        private WheelCollider _forwardLeft;
        [SerializeField]
        private WheelVisual _forwardLeftVisual;
        [SerializeField]
        private WheelCollider _centerLeft;
        [SerializeField]
        private WheelVisual _centerLeftVisual;
        [SerializeField]
        private WheelCollider _backwardLeft;
        [SerializeField]
        private WheelVisual _backwardLeftVisual;

        [SerializeField]
        private WheelCollider _forwardRight;
        [SerializeField]
        private WheelVisual _forwardRightVisual;
        [SerializeField]
        private WheelCollider _centerRight;
        [SerializeField]
        private WheelVisual _centerRightVisual;
        [SerializeField]
        private WheelCollider _backwardRight;
        [SerializeField]
        private WheelVisual _backwardRightVisual;

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

        private Vector3 _ldir23;
        private Vector3 _rdir23;
        private Vector3 _ldir123;
        private Vector3 _rdir123;

        private void Update()
        {
            GetLocalPoses();
            CalculateWheelRotations();
            ApplyVisibility();
        }

        private void CalculateWheelRotations()
        {
            _ldir23 = (_backwardLeftPosition - _centerLeftPosition).normalized;
            Vector3 lmid23 = (_centerLeftPosition + _backwardLeftPosition) * 0.5f;
            _ldir123 = (_forwardLeftPosition - lmid23).normalized;

            _rdir23 = (_backwardRightPosition - _centerRightPosition).normalized;
            Vector3 rmid23 = (_centerRightPosition + _backwardRightPosition) * 0.5f;
            _rdir123 = (_forwardRightPosition - rmid23).normalized;
        }

        private void GetLocalPoses()
        {
            _forwardLeft.GetWorldPose(out _forwardLeftPosition, out _forwardLeftRotation);
            _forwardLeftPosition = transform.InverseTransformPoint(_forwardLeftPosition);
            _forwardRight.GetWorldPose(out _forwardRightPosition, out _forwardRightRotation);
            _forwardRightPosition = transform.InverseTransformPoint(_forwardRightPosition);
            _centerLeft.GetWorldPose(out _centerLeftPosition, out _centerLeftRotation);
            _centerLeftPosition = transform.InverseTransformPoint(_centerLeftPosition);
            _centerRight.GetWorldPose(out _centerRightPosition, out _centerRightRotation);
            _centerRightPosition = transform.InverseTransformPoint(_centerRightPosition);
            _backwardLeft.GetWorldPose(out _backwardLeftPosition, out _backwardLeftRotation);
            _backwardLeftPosition = transform.InverseTransformPoint(_backwardLeftPosition);
            _backwardRight.GetWorldPose(out _backwardRightPosition, out _backwardRightRotation);
            _backwardRightPosition = transform.InverseTransformPoint(_backwardRightPosition);
        }

        private void ApplyVisibility()
        {
            _leftAxle23.localRotation = Quaternion.LookRotation(-_ldir23);
            _leftAxle123.localRotation = Quaternion.LookRotation(_ldir123);

            _rightAxle23.localRotation = Quaternion.LookRotation(-_rdir23);
            _rightAxle123.localRotation = Quaternion.LookRotation(_rdir123);

            _forwardLeftVisual.SetRotation(_forwardLeftRotation);
            _forwardRightVisual.SetRotation(_forwardRightRotation);
            _centerLeftVisual.SetRotation(_centerLeftRotation);
            _centerRightVisual.SetRotation(_centerRightRotation);
            _backwardLeftVisual.SetRotation(_backwardLeftRotation);
            _backwardRightVisual.SetRotation(_backwardRightRotation);
        }
    }
}