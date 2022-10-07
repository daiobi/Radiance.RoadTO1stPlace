using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rover;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class Spec : MonoBehaviour
{
    [SerializeField] private Rover.Wheel _wheel;
    [SerializeField] private Rover.Rover _rover;
    [SerializeField] private int _dst;
    public Image[] _Image;
    public Sprite[] _BrokenSprite;
    public GameObject _gmobj;
    public GameObject _rov;
    public Text _BatteryCharge;
    public Text _health;
    private Sprite[] _defaultSprites;

    private void Start()
    {
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
        _BatteryCharge.text = $"{Mathf.CeilToInt(tel.BatteryPercents * 100f)}%";
        _health.text = $"{tel.Health}";

        _Image[0].sprite = tel.BodyBroken ? _BrokenSprite[0] : _defaultSprites[0];

        _Image[1].sprite = tel.LFBroken ? _BrokenSprite[1] : _defaultSprites[1];
        _Image[2].sprite = tel.LCBroken ? _BrokenSprite[2] : _defaultSprites[2];
        _Image[3].sprite = tel.LBBroken ? _BrokenSprite[3] : _defaultSprites[3];
        _Image[4].sprite = tel.RFBroken ? _BrokenSprite[4] : _defaultSprites[4];
        _Image[5].sprite = tel.RCBroken ? _BrokenSprite[5] : _defaultSprites[5];
        _Image[6].sprite = tel.RBBroken ? _BrokenSprite[6] : _defaultSprites[6];


        if (tel.LBBroken && tel.RFBroken && tel.LCBroken && tel.RCBroken && tel.LBBroken && tel.RBBroken)
        { 
            _rover.TurnOff();
            Debug.Log("Игра окончена");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }


        
    }
    
    


    //using Rover;
    //[SerializeField] private Rover.Rover _rover;
    //_rover.RepairWheel(n);
}
