using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rover
{
    public class DrillTool : MonoBehaviour
    {
        [SerializeField] private Transform _drillJoint;
        [SerializeField] private Transform _drill;
        [SerializeField] private Vector3 _drillRotationSpeed;

        private float _drillSpeed;

        public void SetDrillSpeed(float value)
        {
            _drillSpeed = value;
        }

        private void Update()
        {
            _drill.Rotate(_drillRotationSpeed * _drillSpeed * Time.deltaTime, Space.Self);

        }
    }
}