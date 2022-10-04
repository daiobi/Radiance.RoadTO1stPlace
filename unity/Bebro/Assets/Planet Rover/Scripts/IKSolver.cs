using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rover
{
    public class IKSolver : MonoBehaviour
    {
        public Transform pivot, upper, lower, effector, tip;
        public Transform target;
        public Vector3 normal = Vector3.up;


        float upperLength, lowerLength, effectorLength, pivotLength;
        Vector3 effectorTarget, tipTarget;

        void Reset()
        {
            pivot = transform;
            try
            {
                upper = pivot.GetChild(0);
                lower = upper.GetChild(0);
                effector = lower.GetChild(0);
                tip = effector.GetChild(0);
            }
            catch (UnityException)
            {
                Debug.Log("Could not find required transforms, please assign manually.");
            }
        }

        void Awake()
        {
            upperLength = (lower.position - upper.position).magnitude;
            lowerLength = (effector.position - lower.position).magnitude;
            effectorLength = (tip.position - effector.position).magnitude;
            pivotLength = (upper.position - pivot.position).magnitude;
        }

        void Update()
        {
            tipTarget = target.position;
            effectorTarget = target.position + normal * effectorLength;
            Solve();
        }

        void Solve()
        {
            var pivotDir = pivot.position - effectorTarget;
            pivotDir.y = 0;
            pivot.rotation = Quaternion.LookRotation(pivotDir);

            var upperToTarget = (effectorTarget - upper.position);
            var a = upperLength;
            var b = lowerLength;
            var c = upperToTarget.magnitude;

            var B = Mathf.Acos((c * c + a * a - b * b) / (2 * c * a)) * Mathf.Rad2Deg;
            var C = Mathf.Acos((a * a + b * b - c * c) / (2 * a * b)) * Mathf.Rad2Deg;
            var phi = Mathf.Atan2(effectorTarget.y - upper.position.y, Vector2.Distance(new Vector2(effectorTarget.x, effectorTarget.z), new Vector2(upper.position.x, upper.position.z))) * Mathf.Rad2Deg;

            if (!float.IsNaN(C))
            {
                var upperRotation = Quaternion.AngleAxis(B + phi, Vector3.right);
                upper.localRotation = upperRotation;
                var lowerRotation = Quaternion.AngleAxis(C - 180, Vector3.right);
                lower.localRotation = lowerRotation;
            }
            var effectorRotation = Quaternion.LookRotation(tipTarget - effector.position);
            effector.rotation = effectorRotation;
        }
    }
}