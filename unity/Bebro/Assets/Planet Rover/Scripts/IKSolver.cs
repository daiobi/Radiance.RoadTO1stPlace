using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rover
{
    public class IKSolver : MonoBehaviour
    {
        public Transform effector, tip;
        public PhysicalRotator pivot, upper, lower;
        public Transform target;

        float upperLength, lowerLength;
        Vector3 effectorTarget;

        void Reset()
        {
            pivot = GetComponent<PhysicalRotator>();
            try
            {
                upper = pivot.transform.GetChild(0).GetComponent<PhysicalRotator>();
                lower = upper.transform.GetChild(0).GetComponent<PhysicalRotator>();
                effector = lower.transform.GetChild(0);
                tip = effector.GetChild(0);
            }
            catch (UnityException)
            {
                Debug.Log("Could not find required transforms, please assign manually.");
            }
        }

        void Awake()
        {
            upperLength = (lower.transform.position - upper.transform.position).magnitude;
            lowerLength = (effector.position - lower.transform.position).magnitude;
        }

        void Update()
        {
            effectorTarget = target.position;
            Solve();
        }

        void Solve()
        {
            var upperToTarget = (effectorTarget - upper.transform.position);
            var a = upperLength;
            var b = lowerLength;
            var c = upperToTarget.magnitude;

            var B = Mathf.Acos((c * c + a * a - b * b) / (2 * c * a)) * Mathf.Rad2Deg;
            var C = Mathf.Acos((a * a + b * b - c * c) / (2 * a * b)) * Mathf.Rad2Deg;
            var phi = Mathf.Atan2(effectorTarget.y - upper.transform.position.y, Vector2.Distance(new Vector2(effectorTarget.x, effectorTarget.z), new Vector2(upper.transform.position.x, upper.transform.position.z))) * Mathf.Rad2Deg;

            if (!float.IsNaN(C))
            {
                upper.SetTargetAngle(B + phi);
                lower.SetTargetAngle(C - 180);
            }
        }
    }
}