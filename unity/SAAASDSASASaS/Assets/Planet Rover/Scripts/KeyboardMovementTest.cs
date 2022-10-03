using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rover
{
    [RequireComponent(typeof(RoverMovement))]
    public class KeyboardMovementTest : MonoBehaviour
    {
        private RoverMovement _rover;

        private void Awake()
        {
            _rover = GetComponent<RoverMovement>();
        }

        private void FixedUpdate()
        {
            float acceleration = Input.GetAxis("Vertical");
            float steer = Input.GetAxis("Horizontal");

            _rover.Move(acceleration, steer);
        }
    }
}