using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rover
{
    public class RoverBox : MonoBehaviour
    {
        [SerializeField] private Transform _topPlate;
        [SerializeField] private float _closedPos;
        [SerializeField] private float _openedPos;

        private bool _isClosed = true;

        public BoxState BoxState { get; private set; }

        private void Start()
        {
            _topPlate.localPosition = new Vector3(_closedPos, _topPlate.localPosition.y, _topPlate.localPosition.z);
            BoxState = BoxState.Closed;
        }

        private void Update()
        {
            _topPlate.localPosition = new Vector3(Mathf.MoveTowards(_topPlate.localPosition.x, _isClosed ? _closedPos : _openedPos, Time.deltaTime), _topPlate.localPosition.y, _topPlate.localPosition.z);
        }

        public void Open()
        {
            _isClosed = false;
            if (BoxState == BoxState.Closed)
            {
                BoxState = BoxState.Opened;
            }
        }

        public void Close()
        {
            _isClosed = true;
            BoxState = BoxState.Closed;
        }
    }
}