using UnityEngine;

namespace Rover
{
    public class GrabHandTool : MonoBehaviour
    {
        [SerializeField] private GrabTrigger _grabTrigger;
        [SerializeField] private Transform _rightGrab;
        [SerializeField] private Transform _leftGrab;

        private float _grabValue;

        public void SetGrabValue(float value)
        {
            _grabValue = value;
            _rightGrab.localEulerAngles = new Vector3(0, Mathf.Lerp(0, -20, value), 0);
            _leftGrab.localEulerAngles = new Vector3(0, Mathf.Lerp(0, 20, value), 0);
        }

        private void FixedUpdate()
        {
            if (_grabTrigger.Grabable)
            {
                if (_grabValue > _grabTrigger.Grabable.MaxForce)
                {
                    _grabTrigger.Detach();
                    _grabTrigger.Grabable.BreakDown();
                    Debug.Log("Game over");
                }
                else
                {

                    if (_grabValue > _grabTrigger.Grabable.MinForce)
                    {
                        _grabTrigger.Attach();
                    }
                    else
                    {
                        _grabTrigger.Detach();
                    }
                }
            }
        }
    }
}