﻿using UnityEngine;

namespace Rover
{
    public class RoverBoxes : MonoBehaviour
    {
        [SerializeField] private RoverBox _green;
        [SerializeField] private RoverBox _yellow;
        [SerializeField] private RoverBox _blue;

        public BoxState GreenState => _green.BoxState;
        public BoxState YellowState => _yellow.BoxState;
        public BoxState BlueState => _blue.BoxState;

        public void OpenGreen()
        {
            _green.Open();
            _yellow.Close();
            _blue.Close();
        }

        public void OpenYellow()
        {
            _green.Close();
            _yellow.Open();
            _blue.Close();
        }

        public void OpenBlue()
        {
            _green.Close();
            _yellow.Close();
            _blue.Open();
        }
    }
}