using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Photoable : MonoBehaviour
{
    public float maxDistance;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
}
