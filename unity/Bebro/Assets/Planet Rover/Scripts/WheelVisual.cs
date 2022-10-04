using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rover
{
    public class WheelVisual : MonoBehaviour
    {
        public void SetRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
        }
    }
}