using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rover
{

    public class WheelDamageValidator : MonoBehaviour
    {
        private Dictionary<RoverObstacle, bool> _validObstacles = new Dictionary<RoverObstacle, bool>();

        private void OnTriggerEnter(Collider other)
        {
            RoverObstacle obstacle = other.GetComponent<RoverObstacle>();

            if (obstacle)
            {
                if (!_validObstacles.ContainsKey(obstacle))
                    _validObstacles.Add(obstacle, false);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            RoverObstacle obstacle = other.GetComponent<RoverObstacle>();

            if (obstacle)
            {
                _validObstacles.Remove(obstacle);
            }
        }

        public bool TryBeDamagedBy(RoverObstacle obstacle)
        {
            if (_validObstacles.TryGetValue(obstacle, out bool hadBeenDamaged))
            {
                if (hadBeenDamaged) return false;

                _validObstacles[obstacle] = true;
                return true;
            }

            return false;
        }


    }
}