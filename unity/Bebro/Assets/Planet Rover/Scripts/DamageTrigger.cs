using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Rover
{
    public class DamageTrigger : MonoBehaviour
    {
        [SerializeField] private float _damageSpeed;

        public UnityEvent OnDamage;

        private WheelDamageValidator _damageValidator;

        private void Start()
        {
            _damageValidator = GetComponentInParent<WheelDamageValidator>();
        }

        private void OnTriggerEnter(Collider other)
        {
            RoverObstacle obstacle = other.GetComponent<RoverObstacle>();

            if (obstacle)
            {
                if (_damageValidator.TryBeDamagedBy(obstacle))
                {
                    OnDamage?.Invoke();
                }
            }
        }
    }
}