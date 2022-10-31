using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rover
{
    public class DrillTrigger : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _dustParticles;
        public float Damage { get; set; }

        private List<DrillDestroyable> _destroyables = new List<DrillDestroyable>();

        private void FixedUpdate()
        {
            _destroyables.RemoveAll(x => x == null || x.Destroyed);

            foreach (var destroyable in _destroyables)
            {
                destroyable.TakeDamage(Damage * Time.fixedDeltaTime);
            }


            if (Damage > 0 && _destroyables.Count > 0)
            {
                _dustParticles.Play();
            }
            else if (Damage == 0 || _destroyables.Count == 0)
            {
                _dustParticles.Stop();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            DrillDestroyable destroyable = other.GetComponent<DrillDestroyable>();
            if (destroyable)
            {
                _destroyables.Add(destroyable);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            DrillDestroyable destroyable = other.GetComponent<DrillDestroyable>();
            if (destroyable)
                _destroyables.Remove(destroyable);
        }

        private void OnTriggerStay(Collider other)
        {
            DrillDestroyable destroyable = other.GetComponent<DrillDestroyable>();

            if (destroyable)
            {
                destroyable.TakeDamage(Damage * Time.fixedDeltaTime);

                if (Damage > 0)
                    _dustParticles.Play();
            }
        }
    }
}