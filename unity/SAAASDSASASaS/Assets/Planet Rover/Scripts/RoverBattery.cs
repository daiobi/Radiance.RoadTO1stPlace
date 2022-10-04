using UnityEngine;

namespace Rover
{
    public class RoverBattery : MonoBehaviour
    {
        [SerializeField] private float _maxValue;

        public float Value { get; private set; }

        private void Start()
        {
            Value = _maxValue;
        }

        private void Update()
        {
            Value = Mathf.Max(Value - Time.deltaTime, 0);
        }
    }
}