using Rover;
using UnityEngine;

public class SignalController : MonoBehaviour
{
    [SerializeField] private AnimationCurve _brokenDistance;
    [SerializeField] private AnimationCurve _fixedDistance;

    private float _lerpFactor;
    private bool _radarFixed;

    public float GetSignalValue(float distance)
    {
        
        return Mathf.Lerp(_brokenDistance.Evaluate(distance), _fixedDistance.Evaluate(distance), _lerpFactor);
    }

    private void Update()
    {
        _lerpFactor = Mathf.Lerp(_lerpFactor, Tasks.Instance.RadarFixed ? 1f : 0f, Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _brokenDistance.keys[_brokenDistance.keys.Length - 1].time);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _fixedDistance.keys[_fixedDistance.keys.Length - 1].time);
    }
}
