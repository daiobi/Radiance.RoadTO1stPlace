using UnityEngine;
using UnityEngine.Events;

namespace Rover
{
    public class RoverHealth : MonoBehaviour
    {
        [SerializeField] private RoverBodyHealth _bodyHealth;
        [SerializeField] private Wheel _LFWheel;
        [SerializeField] private Wheel _RFWheel;
        [SerializeField] private Wheel _LCWheel;
        [SerializeField] private Wheel _RCWheel;
        [SerializeField] private Wheel _LBWheel;
        [SerializeField] private Wheel _RBWheel;

        public class WheelEvent : UnityEvent<int> { }
        public WheelEvent OnWheelBroken;
        public UnityEvent OnBodyBroken;


        public int HitCount { get; private set; } = 0;

        public bool IsBodyBroken { get => _bodyHealth.IsBroken; }
        public bool IsLFWheelBroken { get => _LFWheel.IsBroken; }
        public bool IsRFWheelBroken { get => _RFWheel.IsBroken; }
        public bool IsLCWheelBroken { get => _LCWheel.IsBroken; }
        public bool IsRCWheelBroken { get => _RCWheel.IsBroken; }
        public bool IsLBWheelBroken { get => _LBWheel.IsBroken; }
        public bool IsRBWheelBroken { get => _RBWheel.IsBroken; }

        public void RepairWheel(int n)
        {
            switch(n)
            {
                case 1:
                    _LFWheel.Repair();
                    break;
                case 2:
                    _LCWheel.Repair();
                    break;
                case 3:
                    _LBWheel.Repair();
                    break;
                case 4:
                    _RFWheel.Repair();
                    break;
                case 5:
                    _RCWheel.Repair();
                    break;
                case 6:
                    _RBWheel.Repair();
                    break;
                default:
                    throw new System.InvalidOperationException();
            }
        }

        private void OnEnable()
        {
            _bodyHealth.OnBroken += HandleBodyBroken;
            _LFWheel.OnBroken += HandleWheelBroken;
            _RFWheel.OnBroken += HandleWheelBroken;
            _LCWheel.OnBroken += HandleWheelBroken;
            _RCWheel.OnBroken += HandleWheelBroken;
            _LBWheel.OnBroken += HandleWheelBroken;
            _RBWheel.OnBroken += HandleWheelBroken;
        }

        private void OnDisable()
        {
            _bodyHealth.OnBroken -= HandleBodyBroken;
            _LFWheel.OnBroken -= HandleWheelBroken;
            _RFWheel.OnBroken -= HandleWheelBroken;
            _LCWheel.OnBroken -= HandleWheelBroken;
            _RCWheel.OnBroken -= HandleWheelBroken;
            _LBWheel.OnBroken -= HandleWheelBroken;
            _RBWheel.OnBroken -= HandleWheelBroken;
        }

        private void HandleBodyBroken()
        {
            HitCount++;
            OnBodyBroken?.Invoke();
        }

        private void HandleWheelBroken(int n)
        {
            HitCount++;
            OnWheelBroken?.Invoke(n);
        }
    }
}