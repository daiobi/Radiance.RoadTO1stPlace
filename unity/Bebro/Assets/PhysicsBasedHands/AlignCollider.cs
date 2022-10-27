using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class AlignCollider : MonoBehaviour
{
    [SerializeField] private Transform _alignTransform;

    private SphereCollider _collider;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        _collider.center = transform.InverseTransformPoint(_alignTransform.position);
    }
}
