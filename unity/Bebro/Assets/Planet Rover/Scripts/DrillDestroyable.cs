using System.Collections;
using UnityEngine;

namespace Rover
{
    public class DrillDestroyable : MonoBehaviour
    {
        [SerializeField] private float _maxHealth;
        [SerializeField] private ParticleSystem _destroyParticles;

        public bool Destroyed { get; private set; } = false;

        private float _currentHealth;

        private void Awake()
        {
            _currentHealth = _maxHealth;
        }

        public void TakeDamage(float amount)
        {
            if (Destroyed) return;

            _currentHealth -= amount;
            if (_currentHealth <= 0)
            {
                StartCoroutine(DestroyRoutine());
            }
        }

        private IEnumerator DestroyRoutine()
        {
            Destroyed = true;
            Destroy(GetComponent<MeshRenderer>());
            Destroy(GetComponent<MeshCollider>());
            _destroyParticles.Play();
            yield return new WaitForSeconds(_destroyParticles.main.duration);

            Destroy(gameObject);
        }
    }
}