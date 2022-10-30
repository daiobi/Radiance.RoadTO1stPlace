using UnityEngine;

namespace Rover
{
    [RequireComponent(typeof(Rigidbody))]
    public class Grabable : MonoBehaviour
    {
        [SerializeField] private float _minForce = 0.3f;
        [SerializeField] private float _maxForce = 0.5f;
        [SerializeField] private RoverBox.SampleKind _sampleKind;

        private Rigidbody _rigidbody;

        public Rigidbody Rigidbody => _rigidbody;
        public RoverBox.SampleKind SampleKind => _sampleKind;
        public float MinForce => _minForce;
        public float MaxForce => _maxForce;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void SetColliderEnabled(bool state)
        {
            GetComponent<Collider>().enabled = state;
        }

        public void BreakDown()
        {
            
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            GameStatistics.Instance.BrokenSamples++;
        }
    }
}