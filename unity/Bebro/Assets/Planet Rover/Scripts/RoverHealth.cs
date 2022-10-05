using UnityEngine;

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

        public int HitCount { get; private set; }

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
            _bodyHealth.OnBroken.AddListener(CountHit);
            _LFWheel.OnBroken.AddListener(CountHit);
            _RFWheel.OnBroken.AddListener(CountHit);
            _LCWheel.OnBroken.AddListener(CountHit);
            _RCWheel.OnBroken.AddListener(CountHit);
            _LBWheel.OnBroken.AddListener(CountHit);
            _RBWheel.OnBroken.AddListener(CountHit);
        }

        private void OnDisable()
        {
            _bodyHealth.OnBroken.RemoveListener(CountHit);
            _LFWheel.OnBroken.RemoveListener(CountHit);
            _RFWheel.OnBroken.RemoveListener(CountHit);
            _LCWheel.OnBroken.RemoveListener(CountHit);
            _RCWheel.OnBroken.RemoveListener(CountHit);
            _LBWheel.OnBroken.RemoveListener(CountHit);
            _RBWheel.OnBroken.RemoveListener(CountHit);
        }

        private void CountHit()
        {
            HitCount++;
        }
    }
}