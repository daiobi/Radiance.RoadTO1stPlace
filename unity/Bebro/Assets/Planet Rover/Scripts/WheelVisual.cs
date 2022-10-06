﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rover
{
    public class WheelVisual : MonoBehaviour
    {
        [SerializeField] private Transform _wheelJoint;
        private float _rpm;
        private float _steering;

        private void Update()
        {
            transform.Rotate(_rpm / 60 * 360 * Time.deltaTime, 0, 0, Space.Self);

            if (!_wheelJoint)
            {
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, _steering, 0);
            } else
            {
                _wheelJoint.localEulerAngles = new Vector3(0, _steering, 0);
            }
        }

        public void SetValues(float rpm, float steering)
        {
            _rpm = rpm;
            _steering = steering;
        }
    }
}