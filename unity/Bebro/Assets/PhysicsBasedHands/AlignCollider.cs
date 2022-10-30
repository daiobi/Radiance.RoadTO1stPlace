using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AlignCollider : MonoBehaviour
{
    [SerializeField] private Transform _alignTransform;

    private BoxCollider _collider;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        _collider.center = transform.InverseTransformPoint(_alignTransform.position);
    }
}
