﻿using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using System;
using Rover;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(ChangeTools))]
public class Torch : MonoBehaviour
{
    public bool[] _wheely;
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _turnOnSound;
    [SerializeField] private GameObject[] _screens;
    [SerializeField] private GameObject[] _errorImages;
    [SerializeField] private float _deathzone = 10;
    [SerializeField] private float _maxAngle = 30;
    [SerializeField] private Rover.Rover _rover;
    [SerializeField] private float _resetSpeed;
    [SerializeField] private bool _isControllingArm;
    [SerializeField] private Btn _Btn;
    [SerializeField] private Spec _spec;
    private ActionBasedController _currentController;

    private float _joystickX;
    private float _joystickY;
    private float _joystickButtonAxis;
    private float _joystickActivate;

    private ChangeTools _changeTools;

    private InputAction _buttonAxis;

    public void Start()
    {
        _changeTools = GetComponent<ChangeTools>();

        Tasks.Instance.OnGameFail.AddListener(HandleGameFail);

        _rover.TurnOff();
        foreach (var i in _errorImages) i.SetActive(false);
        var controls = new XRIDefaultInputActions();
        _buttonAxis = controls.XRIButtons.ButtonAxis;
        controls.Enable();
    }

    private void HandleGameFail(GameFailReason _)
    {
        foreach (var i in _errorImages) i.SetActive(true);
    }

    private void Update()
    {
        foreach (var s in _screens) s.SetActive(_rover.IsActivated);

        GetJoystickValues();

        if (!_currentController)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * _resetSpeed);
        }
        if (_rover.IsActivated == true)
        {
            _rover.SetGrabArmActive(_isControllingArm);
            switch (_changeTools.Selected)
            {
                case ChangeTools.Tool.Wheels:
                    _rover.Move(_joystickY, _joystickX);
                    break;
                case ChangeTools.Tool.Grab:
                    _rover.MoveGrabArm(_joystickX, _joystickButtonAxis, _joystickY);
                    _rover.SetArmGrab(_joystickActivate);
                    break;
                case ChangeTools.Tool.Drill:
                    _rover.MoveDrillArm(_joystickX, _joystickButtonAxis, _joystickY);
                    _rover.SetDrillSpeed(_joystickActivate);
                    break;
            }
        }

    }

    private void GetJoystickValues()
    {
        _joystickY = transform.localEulerAngles.x;
        if (_joystickY > 180) _joystickY = _joystickY - 360;
        if (-_deathzone < _joystickY && _joystickY < _deathzone) _joystickY = 0;

        _joystickX = transform.localEulerAngles.z;
        if (_joystickX > 180) _joystickX = _joystickX - 360;
        if (-_deathzone < _joystickX && _joystickX < _deathzone) _joystickX = 0;

        _joystickX /= _maxAngle;
        _joystickY /= -_maxAngle;

        _joystickActivate = _currentController ? _currentController.activateActionValue.action.ReadValue<float>() : 0f;
        _joystickButtonAxis = _currentController ? _buttonAxis.ReadValue<float>() : 0f;
    }

    public void StartSelect(SelectEnterEventArgs args)
    {
        _currentController = args.interactorObject.transform.GetComponent<ActionBasedController>();
    }

    public void EndSelect()
    {
        _currentController = null;
    }

    public void RoverState_()
    {
        if (_rover.IsActivated)
        {
            _rover.TurnOff();
        } else
        {
            if (_rover.TurnOn())
            {
                _source.PlayOneShot(_turnOnSound);
            }
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
        _rover.OpenRedBox();
    }


    public void Repair(int n)
    {
        if (_rover.RepairWheel(n))
        {
            _spec.HandleWheelFix(n);
        }
    }

}
