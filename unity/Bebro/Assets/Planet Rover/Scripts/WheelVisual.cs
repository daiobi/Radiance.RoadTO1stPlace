using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rover
{
    public class WheelVisual : MonoBehaviour
    {
        [SerializeField] private Transform _wheelJoint;
        public void SetRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
        }
    }
}