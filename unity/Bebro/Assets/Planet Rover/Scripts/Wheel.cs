using UnityEngine;

namespace Rover
{
    [RequireComponent(typeof(WheelCollider))]
    public class Wheel : MonoBehaviour
    {
        private const float _speedToTorque = 3f;

        [SerializeField] private AnimationCurve _torqueCurve;
        [SerializeField] private DamageTrigger _damageTrigger;
        [SerializeField] private float _brakeForce;
        [SerializeField] private float _steeringAngle;
        [SerializeField] private int _n;
        [SerializeField] private bool _isBroken;
        [SerializeField] private float _maxSpeed;

        public int SpeedReduction { get; set; } = 0;

        public delegate void WheelEvent(int n);
        public event WheelEvent OnBroken;

        private WheelCollider _wheelCollider;
        private float _torque;

        public bool IsBroken => _isBroken;
        public bool WasRepaired { get; private set; } = false;

        private void Awake()
        {
            _wheelCollider = GetComponent<WheelCollider>();
        }

        private void OnEnable()
        {
            _damageTrigger.OnDamage.AddListener(TakeDamage);
        }

        private void OnDisable()
        {
            _damageTrigger.OnDamage.RemoveListener(TakeDamage);
        }

        private void Update()
        {
            _wheelCollider.motorTorque = _torqueCurve.Evaluate(_wheelCollider.rpm) * Mathf.Max(0, _maxSpeed - SpeedReduction) * _speedToTorque * _torque;
        }


        public bool Repair()
        {
            if (CanRepair())
            {
                _isBroken = false;
                WasRepaired = true;
                return true;
            }

            return false;
        }

        public bool CanRepair()
        {
            return IsBroken && !WasRepaired;
        }

        public void SetTorque(float torque)
        {
            if (torque == 0 && !IsBroken)
            {
                _wheelCollider.brakeTorque = _brakeForce;
            } else
            {
                _wheelCollider.brakeTorque = 0;
            }

            _torque = IsBroken ? 0f : torque;
        }

        public void SetSteering(float steering)
        {
            if (!IsBroken) _wheelCollider.steerAngle = steering * _steeringAngle;
        }
        private void TakeDamage()
        {
            if (!IsBroken) {
                _isBroken = true;
                OnBroken?.Invoke(_n);
                Debug.Log($"{name} broken");
            }
        }
    }
}