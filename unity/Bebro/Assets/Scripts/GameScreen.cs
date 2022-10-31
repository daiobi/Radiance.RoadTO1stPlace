using Rover;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Tasks))]
public class GameScreen : MonoBehaviour
{
    [SerializeField] private GameObject _winScreen;
    [SerializeField] private GameObject _loseScreen;
    [SerializeField] private GameObject[] _statsScreens;
    [SerializeField] private TMPro.TextMeshProUGUI _timeStats;
    [SerializeField] private TMPro.TextMeshProUGUI _roverStats;
    [SerializeField] private TMPro.TextMeshProUGUI _signalStats;
    [SerializeField] private TMPro.TextMeshProUGUI _radarStats;
    [SerializeField] private TMPro.TextMeshProUGUI _samplesStats;
    [SerializeField] private Image _radarPhoto;
    [SerializeField] private Image _samplesPhoto;

    [SerializeField] private TextMeshProUGUI _text;

    private void Start()
    {
        var tasks = GetComponent<Tasks>();

        tasks.OnGameFail.AddListener(HandleGameFail);
        tasks.OnGameSuccess.AddListener(HandleGameSuccess);

        _winScreen.SetActive(false);
        _loseScreen.SetActive(false);
        _text.text = "";

        _radarPhoto.enabled = false;
        _samplesPhoto.enabled = false;

        foreach (var s in _statsScreens) s.SetActive(false);
    }

    private void HandleGameSuccess()
    {
        _winScreen.SetActive(true);
        SetupStats();
    }

    private void SetupStats()
    {
        foreach (var s in _statsScreens) s.SetActive(true);
        _timeStats.text = GameStatistics.Instance.GetTimeStats();
        _roverStats.text = GameStatistics.Instance.GetBreakStats();
        _signalStats.text = GameStatistics.Instance.GetSignalStats();
        _radarStats.text = GameStatistics.Instance.GetRadarStats();
        _samplesStats.text = GameStatistics.Instance.GetSamplesCollectStats();

        if (GameStatistics.Instance.RadarPhoto)
        {
            Texture2D texture = GameStatistics.Instance.RadarPhoto;
            _radarPhoto.enabled = true;
            _radarPhoto.sprite = Sprite.Create(texture, new Rect(Vector2.zero, new Vector2(texture.width, texture.height)), new Vector2(texture.width / 2, texture.height / 2));
        }

        if (GameStatistics.Instance.SamplesPhoto)
        {
            Texture2D texture = GameStatistics.Instance.SamplesPhoto;
            _samplesPhoto.enabled = true;
            _samplesPhoto.sprite = Sprite.Create(texture, new Rect(Vector2.zero, new Vector2(texture.width, texture.height)), new Vector2(texture.width / 2, texture.height / 2));
        }
    }

    private void HandleGameFail(GameFailReason arg0)
    {
        _loseScreen.SetActive(true);
        SetupStats();
        Debug.Log(arg0.GetType().Name);

        if (arg0 is InvalidAction)
        {
            _text.text = "неверная последовательность действий";
        }
        else if (arg0 is SampleBroken)
        {
            _text.text = "образец уничтожен";
        }
        else
        {
            var roverBreakReason = (RoverBrokenDown)arg0;
            switch (roverBreakReason.BreakDownCase)
            {
                case Rover.Rover.BreakDownCause.BatteryLow:
                    _text.text = "заряд аккумулятора достиг критического значения";
                    break;
                case Rover.Rover.BreakDownCause.Distance:
                    _text.text = "связь с ровером потеряна";
                    break;
                case Rover.Rover.BreakDownCause.Flip:
                    _text.text = "ровер перевернулся";
                    break;
                case Rover.Rover.BreakDownCause.Health:
                    _text.text = "ровер уничтожен";
                    break;
            }
        }
    }
}
