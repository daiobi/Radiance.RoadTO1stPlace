﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Rover;

public class Spec : MonoBehaviour
{
    [SerializeField] private Material _noiseMaterial;
    [SerializeField] private AnimationCurve _noiseCurve;
    [SerializeField] private Rover.Rover _rover;
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _fixClip;
    [SerializeField] private AudioClip _breakClip;
    [SerializeField] private GameObject[] _statsDisplays;
    [SerializeField] private TMPro.TextMeshProUGUI _statsLeft;
    [SerializeField] private TMPro.TextMeshProUGUI _statsRight;
    public Image[] _Image;
    public Sprite[] _BrokenSprite;
    public Image _ImageSignal;
    public Sprite[] _SpritesSignal;
    public GameObject _gmobj;
    public GameObject _rov;
    public Text _BatteryCharge;
    public Image _health;
    public TMPro.TextMeshProUGUI _speedText;
    public TMPro.TextMeshProUGUI _logText;
    private Sprite[] _defaultSprites;

    private bool[] _lastStatuses;
    private List<string> _log;
    [SerializeField] private Torch _leveler;
    [SerializeField] public RadarMinigame _RadMin;
    [SerializeField] private int m = 2;
    



    private void Start()
    {
        _lastStatuses = new bool[7];
        _log = new List<string>();

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


        if (!_rover.IsActivated) return;

        var signal = tel.Signal;
        _noiseMaterial.SetFloat("_NoiseFactor", Mathf.Lerp(0, 0.9f, Mathf.Clamp01(1f - signal + (tel.Health < 3 ? 0.25f : 0f))));
        if (signal < 0.2f)
            _ImageSignal.sprite = _SpritesSignal[3];
        else if (signal < 0.4f)
            _ImageSignal.sprite = _SpritesSignal[2];
        else if (signal < 0.6f)
            _ImageSignal.sprite = _SpritesSignal[1];
        else
            _ImageSignal.sprite = _SpritesSignal[0];
        
        _speedText.text = $"{(int)tel.Speed} км/ч";
        _BatteryCharge.text = $"{Mathf.CeilToInt(tel.BatteryPercents * 100f)}%";
        _health.fillAmount = Mathf.Lerp(_health.fillAmount, tel.Health / 6f, Time.deltaTime);


        _Image[1].sprite = tel.LFBroken ? _BrokenSprite[1] : _defaultSprites[1];
        _Image[2].sprite = tel.LCBroken ? _BrokenSprite[2] : _defaultSprites[2];
        _Image[3].sprite = tel.LBBroken ? _BrokenSprite[3] : _defaultSprites[3];
        _Image[4].sprite = tel.RFBroken ? _BrokenSprite[4] : _defaultSprites[4];
        _Image[5].sprite = tel.RCBroken ? _BrokenSprite[5] : _defaultSprites[5];
        _Image[6].sprite = tel.RBBroken ? _BrokenSprite[6] : _defaultSprites[6];


        HandleStatuses(tel);

        _logText.text = string.Join("\n", _log);
    }

    private void HandleStatuses(Rover.Telemetry tel)
    {
        if (_lastStatuses[1] != tel.LFBroken)
        {
            _lastStatuses[1] = tel.LFBroken;
            _log.Add($"> Сломано 1 колесо");
            _source.PlayOneShot(_breakClip);
        }
        if (_lastStatuses[2] != tel.LCBroken)
        {
            _lastStatuses[2] = tel.LCBroken;
            _log.Add($"> Сломано 2 колесо");
            _source.PlayOneShot(_breakClip);
        }
        if (_lastStatuses[3] != tel.LBBroken)
        {
            _lastStatuses[3] = tel.LBBroken;
            _log.Add($"> Сломано 3 колесо");
            _source.PlayOneShot(_breakClip);
        }
        if (_lastStatuses[4] != tel.RFBroken)
        {
            _lastStatuses[4] = tel.RFBroken;
            _log.Add($"> Сломано 4 колесо");
            _source.PlayOneShot(_breakClip);
        }
        if (_lastStatuses[5] != tel.RCBroken)
        {
            _lastStatuses[5] = tel.RCBroken;
            _log.Add($"> Сломано 5 колесо");
            _source.PlayOneShot(_breakClip);
        }
        if (_lastStatuses[6] != tel.RBBroken)
        {
            _lastStatuses[6] = tel.RBBroken;
            _log.Add($"> Сломано 6 колесо");
            _source.PlayOneShot(_breakClip);
        }
    }

    public void HandleWheelFix(int n)
    {
        _lastStatuses[n] = false;
        _log.Add($"> Колесо {n} починено");
        _source.PlayOneShot(_fixClip);
    }
    
    


    //using Rover;
    //[SerializeField] private Rover.Rover _rover;
    //_rover.RepairWheel(n);
}
