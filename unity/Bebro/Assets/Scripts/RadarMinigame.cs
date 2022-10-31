using Rover;
using UnityEngine;
using TMPro;

public class RadarMinigame : MonoBehaviour
{

    [SerializeField] private GameObject _minigamePanel;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Transform _radarRotation;
    [SerializeField] private Move _minigame;
    public bool IsWon { get; private set; }

    private void Start()
    {
        _minigame.OnWin.AddListener(HandleWin);
        _minigame.OnFail.AddListener(HandleFail);
    }

    private void Update()
    {
        _text.text = $"{_minigame.CalibVal}/3";

        if (IsWon)
            _radarRotation.rotation = Quaternion.Slerp(_radarRotation.rotation, Quaternion.Euler(0, -90, 0), Time.deltaTime * 0.5f);
    }

    private void HandleWin()
    {
        IsWon = true;
        Tasks.SetRadarFixed();

        CloseMinigame();
    }

    private void HandleFail()
    {
        CloseMinigame();
    }

    public void CloseMinigame()
    {
        _minigamePanel.SetActive(false);
    }

    public void HandleButton()
    {
        if (_minigamePanel.activeInHierarchy)
        {
            _minigame.btn();
        }
    }

    public void OpenMinigame()
    {
        if (!IsWon)
        {
            Tasks.HandleRadarFixStarted();
            _minigame.Restart();
            _minigamePanel.SetActive(true);
        }
    }
}
