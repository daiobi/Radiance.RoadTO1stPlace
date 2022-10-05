﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Rover
{
    [RequireComponent(typeof(WheelCollider))]
    public class Wheel : MonoBehaviour
    {
        [SerializeField] private DamageTrigger _damageTrigger;
        [SerializeField] private float _brakeForce;
        [SerializeField] private float _steeringAngle;
        [SerializeField] private float _motorForce;
        [SerializeField] private int _n;
        [SerializeField] private bool _isBroken;

        public delegate void WheelEvent(int n);
        public event WheelEvent OnBroken;

        private WheelCollider _wheelCollider;

        public bool IsBroken => _isBroken;

        private void Awake()
        {
            _wheelCollider = GetComponent<WheelCollider>();
        }

        private void OnEnable()
        {
            _damageTrigger.OnDamage.AddListener(TakeDamage);
        }

        private void OnDisable()
        {
            _damageTrigger.OnDamage.RemoveListener(TakeDamage);
        }


        public void Repair()
        {
            _isBroken = false;
        }

        public void SetTorque(float torque)
        {
            if (torque == 0 || IsBroken)
            {
                _wheelCollider.brakeTorque = _brakeForce;
            } else
            {
                _wheelCollider.brakeTorque = 0;
                _wheelCollider.motorTorque = torque * _motorForce;
            }
        }

        public void SetSteering(float steering)
        {
            if (!IsBroken) _wheelCollider.steerAngle = steering * _steeringAngle;
        }
        private void TakeDamage()
        {
            if (!IsBroken) {
                _isBroken = true;
                OnBroken?.Invoke(_n);
                Debug.Log($"{name} broken");
            }
        }
    }
}