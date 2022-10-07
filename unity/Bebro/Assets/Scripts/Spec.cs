using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rover;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using TMPro;

public class Spec : MonoBehaviour
{
    [SerializeField] private Rover.Wheel _wheel;
    [SerializeField] public Rover.Rover _rover;
    [SerializeField] private int _dst;
    public Image[] _Image;
    public Sprite[] _BrokenSprite;
    public GameObject _gmobj;
    public GameObject _rov;
    public Text _BatteryCharge;
    public Text _health;
    private Sprite[] _defaultSprites;
    public TextMeshProUGUI _SpeedVal;
    public TextMeshProUGUI _wheelLog;

    public Image _ImageDist;
    public Sprite[] _SignalSprite;

    private List<string> _log;
    private bool[] _lastWheelStatuses;

    private void Start()
    {
        _log = new List<string>();
        _lastWheelStatuses = new bool[7];

        _defaultSprites = new Sprite[7];
        _defaultSprites[0] = _Image[0].sprite;
        _defaultSprites[1] = _Image[1].sprite;
        _defaultSprites[2] = _Image[2].sprite;
        _defaultSprites[3] = _Image[3].sprite;
        _defaultSprites[4] = _Image[4].sprite;
        _defaultSprites[5] = _Image[5].sprite;
        _defaultSprites[6] = _Image[6].sprite;
    }

    private void Update()
    {

        var tel = _rover.GetTelemetry();
        var dist = (int)Vector3.Distance(_rover.transform.position, _gmobj.transform.position);
        _BatteryCharge.text = $"{Mathf.CeilToInt(tel.BatteryPercents * 100f)}%";
        _health.text = $"{tel.Health}";
        _SpeedVal.text = $"{(int)tel.Speed} км/ч";
        var _distance = (int)Vector3.Distance(_rover.transform.position, _gmobj.transform.position);
        print(_distance);
        if (_distance >= 80)
            _ImageDist.sprite = _SignalSprite[3];
        else if (_distance >= 60)
            _ImageDist.sprite = _SignalSprite[2];
        else if (_distance >= 40)
            _ImageDist.sprite = _SignalSprite[1];
        else if (_distance <= 20)
            _ImageDist.sprite = _SignalSprite[0];

        _Image[0].sprite = tel.BodyBroken ? _BrokenSprite[0] : _defaultSprites[0];

        _Image[1].sprite = tel.LFBroken ? _BrokenSprite[1] : _defaultSprites[1];
        _Image[2].sprite = tel.LCBroken ? _BrokenSprite[2] : _defaultSprites[2];
        _Image[3].sprite = tel.LBBroken ? _BrokenSprite[3] : _defaultSprites[3];
        _Image[4].sprite = tel.RFBroken ? _BrokenSprite[4] : _defaultSprites[4];
        _Image[5].sprite = tel.RCBroken ? _BrokenSprite[5] : _defaultSprites[5];
        _Image[6].sprite = tel.RBBroken ? _BrokenSprite[6] : _defaultSprites[6];

        HandleWheelBrokenLog(tel);

        _wheelLog.text = string.Join("\n", _log);
    }

    private void HandleWheelBrokenLog(Telemetry tel)
    {
        if (_lastWheelStatuses[0] != tel.BodyBroken)
        {
            _lastWheelStatuses[0] = tel.BodyBroken;
            _log.Add($"> Сломан корпус");
        }

        if (_lastWheelStatuses[1] != tel.LFBroken)
        {
            _lastWheelStatuses[1] = tel.LFBroken;
            _log.Add($"> Сломано колесо 1");
        }

        if (_lastWheelStatuses[2] != tel.LCBroken)
        {
            _lastWheelStatuses[2] = tel.LCBroken;
            _log.Add($"> Сломано колесо 2");
        }

        if (_lastWheelStatuses[3] != tel.LBBroken)
        {
            _lastWheelStatuses[3] = tel.LBBroken;
            _log.Add($"> Сломано колесо 3");
        }

        if (_lastWheelStatuses[4] != tel.RFBroken)
        {
            _lastWheelStatuses[4] = tel.RFBroken;
            _log.Add($"> Сломано колесо 4");
        }

        if (_lastWheelStatuses[5] != tel.RCBroken)
        {
            _lastWheelStatuses[5] = tel.RCBroken;
            _log.Add($"> Сломано колесо 5");
        }

        if (_lastWheelStatuses[6] != tel.RBBroken)
        {
            _lastWheelStatuses[6] = tel.RBBroken;
            _log.Add($"> Сломано колесо 6");
        }
    }

    public void OnWheelRepaired(int n)
    {
        _log.Add($"> Починено колесо {n}");
        _lastWheelStatuses[n] = false;
    }
    
    


    //using Rover;
    //[SerializeField] private Rover.Rover _rover;
    //_rover.RepairWheel(n);
}
