using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rover
{
    [RequireComponent(typeof(RoverMovement))]
    public class KeyboardMovementTest : MonoBehaviour
    {
        private RoverMovement _rover;
        private float _acceleration;
        private float _steer;

        private void Awake()
        {
            _rover = GetComponent<RoverMovement>();
        }

        private void Update()
        {
            _acceleration = Input.GetAxis("Vertical");
            _steer = Input.GetAxis("Horizontal");
        }

        private void FixedUpdate()
        {
            _rover.Move(_acceleration, _steer);
        }
    }
}