using UnityEngine;

namespace Rover
{
    [RequireComponent(typeof(WheelCollider))]
    public class Wheel : MonoBehaviour
    {
        [SerializeField] private AnimationCurve _torqueCurve;
        [SerializeField] private DamageTrigger _damageTrigger;
        [SerializeField] private float _brakeForce;
        [SerializeField] private float _steeringAngle;
        [SerializeField] private int _n;
        [SerializeField] private bool _isBroken;

       
        public delegate void WheelEvent(int n);
        public event WheelEvent OnBroken;

        private WheelCollider _wheelCollider;

        public bool IsBroken => _isBroken;
        public float Torque { get; set; }

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
            _wheelCollider.motorTorque = _torqueCurve.Evaluate(_wheelCollider.rpm) * Torque;
            _wheelCollider.brakeTorque = (IsBroken || Torque == 0) ? _brakeForce : 0f;
        }


        public bool Repair()
        {
            if (IsBroken)
            {
                _isBroken = false;
                return true;
            }

            return false;
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
                GameStatistics.Instance.RegisterEvent(new WheelBrokenRecord(_n));
                Debug.Log($"{name} broken");
            }
        }
    }
}