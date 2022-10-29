using UnityEngine;

public class AlignTransform : MonoBehaviour
{
    [SerializeField] private Transform _trackedTransform;

    private void Update()
    {
        transform.position = _trackedTransform.position;
        transform.rotation = _trackedTransform.rotation;
    }
}
