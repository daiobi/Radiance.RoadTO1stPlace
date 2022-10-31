using UnityEngine;

namespace Rover
{
    public class DrillTool : MonoBehaviour
    {
        [SerializeField] private Transform _drillJoint;
        [SerializeField] private Transform _drill;
        [SerializeField] private Vector3 _drillRotationSpeed;
        [SerializeField] private DrillTrigger _drillTrigger;
        [SerializeField] private float _drillDamage;

        private bool _drillState;

        public void SetActivated(bool state)
        {
            _drillState = state;
        }

        private void Update()
        {
            _drill.Rotate(_drillRotationSpeed * (_drillState ? 1 : 0) * Time.deltaTime, Space.Self);
            _drillTrigger.Damage = _drillState ? _drillDamage : 0;
        }
    }
}