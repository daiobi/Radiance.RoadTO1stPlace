using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rover;

public class Torch : MonoBehaviour
{
    [SerializeField] private float _deathzone = 10;
    [SerializeField] private float _maxAngle = 30;
    [SerializeField] private Rover.Rover _rover;
    [SerializeField] private float _resetSpeed;
    private bool _isSelected;

    private void Start()
    {
        _rover.TurnOn();
    }

    private void Update()
    {
        if (!_isSelected)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * _resetSpeed);
        }

        float y = transform.rotation.eulerAngles.x;
        if (y > 180) y = y - 360;
        if (-_deathzone < y && y < _deathzone) y = 0;

        float x = transform.rotation.eulerAngles.z;
        if (x > 180) x = x - 360;
        if (-_deathzone < x && x < _deathzone) x = 0;

        x /= _maxAngle;
        y /= -_maxAngle;

        Debug.Log($"{x}x, {y}y");
        _rover.Move(y, x);
    }

    public void StartSelect()
    {
        _isSelected = true;
    }

    public void EndSelect()
    {
        _isSelected = false;
    }
}
