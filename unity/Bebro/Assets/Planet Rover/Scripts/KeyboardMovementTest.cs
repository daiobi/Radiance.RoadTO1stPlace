using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rover
{
    [RequireComponent(typeof(RoverMovement))]
    public class KeyboardMovementTest : MonoBehaviour
    {
        private RoverMovement _roverMovement;
        private RoverArm _roverArm;
        private float _acceleration;
        private float _steer;

        private void Awake()
        {
            _roverMovement = GetComponent<RoverMovement>();
            _roverArm = GetComponent<RoverArm>();
        }

        private void Update()
        {
            _acceleration = Input.GetAxis("Vertical");
            _steer = Input.GetAxis("Horizontal");

            if (Input.GetKeyDown(KeyCode.F))
            {
                _roverArm.Move(-1, 0, 0);
            }
            else if (Input.GetKeyDown(KeyCode.H))
            {
                _roverArm.Move(1, 0, 0);
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                _roverArm.Move(0, 0, 1);
            }
            else if (Input.GetKeyDown(KeyCode.G))
            {
                _roverArm.Move(0, 0, -1);
            }

            if (Input.GetKeyDown(KeyCode.U))
            {
                _roverArm.Move(0, 1, 0);
            }
            else if (Input.GetKeyDown(KeyCode.J))
            {
                _roverArm.Move(0, -1, 0);
            }
        }

        private void FixedUpdate()
        {
            _roverMovement.Move(_acceleration, _steer);
        }
    }
}