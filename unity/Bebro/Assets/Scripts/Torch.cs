using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using System;
using Rover;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(ChangeTools))]
public class Torch : MonoBehaviour
{
    [SerializeField] private Camera _screenshotCamera;
    [SerializeField] private Photoable _radarPhotoable;
    [SerializeField] private Photoable _samplesPhotoable;
    [SerializeField] private GameObject _cameraIcon;

    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _turnOnSound;
    [SerializeField] private GameObject[] _screens;
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

        _rover.TurnOff();
        var controls = new XRIDefaultInputActions();
        _buttonAxis = controls.XRIButtons.ButtonAxis;
        controls.Enable();
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

        HandleCameraIcon();

    }

    private void HandleCameraIcon()
    {
        if (CanTakeRadarPhoto())
            _cameraIcon.SetActive(true);
        else if (CanTakeSamplesPhoto())
            _cameraIcon.SetActive(true);
        else
            _cameraIcon.SetActive(false);
    }

    private bool CanTakeRadarPhoto()
    {
        float toRadarDistance = Vector3.Distance(_rover.transform.position, _radarPhotoable.transform.position);

        if ((Tasks.Instance.RadarFixed && !GameStatistics.Instance.RadarPhoto) && toRadarDistance <= _radarPhotoable.maxDistance)
        {
            Vector3 viewportPoint = _screenshotCamera.WorldToViewportPoint(_radarPhotoable.transform.position);

            if (viewportPoint.x > 0.3f && viewportPoint.x < 0.7f && viewportPoint.y > 0.3f && viewportPoint.y < 0.7f && viewportPoint.z > 0f)
            {
                return true;
            }
        }

        return false;
    }

    private bool CanTakeSamplesPhoto()
    {
        float toSamplesDistance = Vector3.Distance(_rover.transform.position, _samplesPhotoable.transform.position);

        if ((Tasks.Instance.GamePhase == GamePhase.SamplesCollected && !GameStatistics.Instance.SamplesPhoto) && toSamplesDistance <= _samplesPhotoable.maxDistance)
        {
            Vector3 viewportPoint = _screenshotCamera.WorldToViewportPoint(_samplesPhotoable.transform.position);

            if (viewportPoint.x > 0.3f && viewportPoint.x < 0.7f && viewportPoint.y > 0.3f && viewportPoint.y < 0.7f && viewportPoint.z > 0f)
            {
                return true;
            }
        }

        return false;
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

    public void TakeScreenShot()
    {
        if (CanTakeRadarPhoto())
        {
            GameStatistics.Instance.RadarPhoto = TakePhoto();
        }
        else if (CanTakeSamplesPhoto())
        {
            GameStatistics.Instance.SamplesPhoto = TakePhoto();
        }
    }

    private Texture2D TakePhoto()
    {
        // The Render Texture in RenderTexture.active is the one
        // that will be read by ReadPixels.
        var currentRT = RenderTexture.active;
        RenderTexture.active = _screenshotCamera.targetTexture;

        // Render the camera's view.
        _screenshotCamera.Render();

        // Make a new texture and read the active Render Texture into it.
        Texture2D image = new Texture2D(_screenshotCamera.targetTexture.width, _screenshotCamera.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, _screenshotCamera.targetTexture.width, _screenshotCamera.targetTexture.height), 0, 0);
        image.Apply();

        // Replace the original active Render Texture.
        RenderTexture.active = currentRT;

        return image;
    }
}
