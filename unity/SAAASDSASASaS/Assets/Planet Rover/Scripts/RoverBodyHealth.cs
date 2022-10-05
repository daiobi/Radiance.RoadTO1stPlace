using UnityEngine;
using UnityEngine.Events;

namespace Rover
{
    public class RoverBodyHealth : MonoBehaviour
    {
        [SerializeField] private DamageTrigger _damageTrigger;
        public UnityEvent OnBroken;

        public bool IsBroken { get; private set; } = false;

        private void OnEnable()
        {
            _damageTrigger.OnDamage.AddListener(TakeDamage);
        }

        private void OnDisable()
        {
            _damageTrigger.OnDamage.RemoveListener(TakeDamage);
        }

        private void TakeDamage()
        {
            if (!IsBroken)
            {
                IsBroken = true;
                OnBroken?.Invoke();
            }
        }
    }
}