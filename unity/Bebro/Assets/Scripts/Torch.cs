using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rover;

public class Torch : MonoBehaviour
{
    [SerializeField] private GameObject[] _screens;
    [SerializeField] private float _deathzone = 10;
    [SerializeField] private float _maxAngle = 30;
    [SerializeField] private Rover.Rover _rover;
    [SerializeField] private float _resetSpeed;
    [SerializeField] private bool set_;
    [SerializeField] private Btn _Btn;
    private bool _isSelected;
    

    public void Start()
    {
        _rover.TurnOff();
    }

    private void Update()
    {
        foreach (var s in _screens) s.SetActive(_rover.IsActivated);

        if (!_isSelected)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * _resetSpeed);
        }
        if (_rover.IsActivated == true)
        {
            _rover.TurnOn();

            float y = transform.rotation.eulerAngles.x;
            if (y > 180) y = y - 360;
            if (-_deathzone < y && y < _deathzone) y = 0;

            float x = transform.rotation.eulerAngles.z;
            if (x > 180) x = x - 360;
            if (-_deathzone < x && x < _deathzone) x = 0;

            x /= _maxAngle;
            y /= -_maxAngle;
            


            if (set_ == false)
            {
                Debug.Log($"{x}x, {y}y");
                _rover.Move(y, x);

            }
            else
            {
                //...
                _rover.MoveArm(x, 0, y);
            }
        }
        else
        {
            _rover.TurnOff();
        }

    }

    public void StartSelect()
    {
        _isSelected = true;
    }

    public void EndSelect()
    {
        _isSelected = false;
    }

    public void WwqWwwEeeEewQwe()
    {
        set_ = !set_; //False --> True
    }

    public void RoverState_()
    {
        if (_rover.IsActivated)
        {
            _rover.TurnOff();
        } else
        {
            _rover.TurnOn();
        }
    }

    public void OpenGreen()
    {
        _rover.OpenGreenBox();
    }

    public void OpenYellow()
    {
        _rover.OpenYellowBox();
    }
    public void OpenBlue()
    {
        _rover.OpenBlueBox();
    }


    public void Repair(int n)
    {
        _rover.RepairWheel(n);
    }
}
